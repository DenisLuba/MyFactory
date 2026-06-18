using MediatR;
using System;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Receive;

public sealed class ReceiveMaterialPurchaseOrderCommandHandler
    : IRequestHandler<ReceiveMaterialPurchaseOrderCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ReceiveMaterialPurchaseOrderCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }
    public async Task Handle(
        ReceiveMaterialPurchaseOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _db.MaterialPurchaseOrders
            .Include(o => o.MaterialPurchaseItems)
            .FirstOrDefaultAsync(o => o.Id == request.PurchaseOrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException("Purchase order not found");

        // map order items by material id and by item id for lookup (fallback)
        var orderItemsByMaterial = order.MaterialPurchaseItems.ToDictionary(i => i.MaterialId);
        var orderItemsById = order.MaterialPurchaseItems.ToDictionary(i => i.Id);

        // Filter out empty material lines (user may have left selection blank) and validate allocations
        var validAllocations = request.Allocations
            .Select(a => new
            {
                Allocation = a,
                MaterialLines = (a.MateialItems ?? Array.Empty<ReceiveMaterialPurchaseOrderItem>())
                    .Where(mi => mi.MaterialId != Guid.Empty && mi.QtyPerPackage > 0)
                    .ToList()
            })
            .ToList();

        foreach (var va in validAllocations)
        {
            if (va.Allocation.WarehouseId == Guid.Empty)
                throw new DomainApplicationException("Allocation contains empty WarehouseId.");
        }

        // remove allocations that contain no valid material lines (user left selections empty)
        validAllocations = validAllocations.Where(va => va.MaterialLines.Count > 0).ToList();
        if (validAllocations.Count == 0)
            throw new DomainApplicationException("No valid allocations provided.");

        // collect allocations per provided id across all warehouses (incoming ids may be either MaterialId or ItemId)
        var incomingGroups = validAllocations
            .SelectMany(a => a.MaterialLines)
            .GroupBy(m => m.MaterialId)
            .ToList();

        var allocatedByMaterial = new Dictionary<Guid, decimal>();
        foreach (var g in incomingGroups)
        {
            var key = g.Key;
            var sumQty = g.Sum(x => CalculateTotal(x.QtyPerPackage, x.PackageCount));

            if (orderItemsByMaterial.ContainsKey(key))
            {
                // key is material id
                allocatedByMaterial[key] = allocatedByMaterial.GetValueOrDefault(key) + sumQty;
            }
            else if (orderItemsById.ContainsKey(key))
            {
                // key is order item id � map to material id on the order
                var mappedMaterialId = orderItemsById[key].MaterialId;
                allocatedByMaterial[mappedMaterialId] = allocatedByMaterial.GetValueOrDefault(mappedMaterialId) + sumQty;
            }
            else
            {
                throw new DomainApplicationException($"Unknown material or item id {key} in allocations.");
            }
        }

        // Recalculate unit prices by distributing total shipping cost across materials
        // Formula described by business:
        // K = Dt / Ct, where Dt = total shipping, Ct = total materials cost

        var totalMaterialCost = order.MaterialPurchaseItems.Sum(i => i.UnitPrice * i.Qty);
        var totalShipping = request.Allocations.Sum(a => a.ShippingCost ?? 0m);

        decimal k = 0m;
        if (totalMaterialCost > 0 && totalShipping > 0)
            k = totalShipping / totalMaterialCost;

        // For each material: Ci_final = Ci * (1 + K); Price_i = Ci_final / Qi
        foreach (var orderItem in order.MaterialPurchaseItems)
        {
            var ci = orderItem.UnitPrice * orderItem.Qty;
            var ciFinal = ci * (1 + k);
            var newUnitPrice = orderItem.Qty > 0 ? ciFinal / orderItem.Qty : orderItem.UnitPrice;

            orderItem.UpdateUnitPrice(newUnitPrice, order);
        }

        var createdBy = _currentUser.UserId != Guid.Empty
            ? _currentUser.UserId
            : request.ReceivedByUserId;

        if (createdBy == Guid.Empty)
            throw new DomainApplicationException("ReceivedByUserId is required.");

        var userExists = await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == createdBy, cancellationToken);

        if (!userExists)
            throw new DomainApplicationException($"User {createdBy} not found.");

        var movements = new Dictionary<Guid, InventoryMovementEntity>();


        // Validate that allocated quantities per material match order quantities
        const decimal EPS = 0.0001m;
        foreach (var orderItem in order.MaterialPurchaseItems)
        {
            // try match by material id first, then by order item id (client may accidentally send item ids)
            if (!allocatedByMaterial.TryGetValue(orderItem.MaterialId, out var allocatedQty)
                && !allocatedByMaterial.TryGetValue(orderItem.Id, out allocatedQty))
            {
                throw new DomainApplicationException($"Order item for material {orderItem.MaterialId} is not allocated.");
            }

            if (Math.Abs(allocatedQty - orderItem.Qty) > EPS)
                throw new DomainApplicationException($"Allocated quantity for material {orderItem.MaterialId} must equal ordered quantity.");
        }

        // Apply allocations to warehouses and create inventory movements
        // Use filtered validAllocations to avoid processing empty/invalid material lines
        foreach (var va in validAllocations)
        {
            var alloc = va.Allocation;
            if (!movements.TryGetValue(alloc.WarehouseId, out var movement))
            {
                movement = new InventoryMovementEntity(
                    movementType: InventoryMovementType.Receipt,
                    fromWarehouseId: null,
                    toWarehouseId: alloc.WarehouseId,
                    toDepartmentId: null,
                    productionOrderId: null,
                    createdBy: createdBy);

                _db.InventoryMovements.Add(movement);
                movements[alloc.WarehouseId] = movement;
            }

            foreach (var mat in va.MaterialLines)
            {
                // try to find order item by material id, fallback to item id
                if (!orderItemsByMaterial.TryGetValue(mat.MaterialId, out var orderItem))
                {
                    if (!orderItemsById.TryGetValue(mat.MaterialId, out orderItem))
                        throw new NotFoundException($"Order item for material {mat.MaterialId} not found in order");
                }

                var movementItem = new InventoryMovementItemEntity(
                    movementId: movement.Id,
                    materialId: orderItem.MaterialId,
                    qty: CalculateTotal(mat.QtyPerPackage, mat.PackageCount),
                    unitCost: orderItem.UnitPrice
                );

                _db.InventoryMovementItems.Add(movementItem);

                var stock = await _db.WarehouseMaterials
                    .FirstOrDefaultAsync(
                        wm => wm.WarehouseId == alloc.WarehouseId
                        && wm.MaterialId == orderItem.MaterialId,
                        cancellationToken);

                if (stock is null)
                {
                    stock = new WarehouseMaterialEntity(
                        warehouseId: alloc.WarehouseId,
                        materialId: orderItem.MaterialId,
                        qtyPerPackage: mat.QtyPerPackage,
                        packageCount: mat.PackageCount);

                    _db.WarehouseMaterials.Add(stock);
                }
                else
                {
                    stock.AddQty(mat.QtyPerPackage, mat.PackageCount);
                }
            }
        }

        // Set order status after successful allocations
        order.Receive();

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static decimal CalculateTotal(decimal qtyPerPackage, decimal? packageCount)
        => packageCount is null ? qtyPerPackage : qtyPerPackage * packageCount.Value;
}
