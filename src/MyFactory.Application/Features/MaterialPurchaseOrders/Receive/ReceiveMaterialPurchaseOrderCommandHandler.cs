using MediatR;
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

        var orderItems = order.MaterialPurchaseItems.ToDictionary(i => i.Id);

        // validate items coverage
        if (request.Items.Count != orderItems.Count)
            throw new DomainApplicationException("All order items must be allocated to complete receipt.");

        foreach (var reqItem in request.Items)
        {
            if (!orderItems.TryGetValue(reqItem.ItemId, out var orderItem))
                throw new NotFoundException($"Order item {reqItem.ItemId} not found in order");

            var allocated = reqItem.Allocations.Sum(a => a.Qty);
            if (allocated != orderItem.Qty)
                throw new DomainApplicationException("Allocated quantity must equal ordered quantity for each item.");
        }

        var createdBy = request.ReceivedByUserId != Guid.Empty
            ? request.ReceivedByUserId
            : _currentUser.UserId;

        var movements = new Dictionary<Guid, InventoryMovementEntity>();

        foreach (var reqItem in request.Items)
        {
            var orderItem = orderItems[reqItem.ItemId];

            foreach (var alloc in reqItem.Allocations)
            {
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

                var movementItem = new InventoryMovementItemEntity(
                    movementId: movement.Id,
                    materialId: orderItem.MaterialId,
                    qty: alloc.Qty,
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
                        qty: alloc.Qty
                    );

                    _db.WarehouseMaterials.Add(stock);
                }
                else
                {
                    stock.AddQty(alloc.Qty);
                }
            }
        }

        // Set order status after successful allocations
        order.Receive();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
