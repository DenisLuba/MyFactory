using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.Warehouses.TransferMaterials;

public sealed class TransferMaterialsCommandHandler
    : IRequestHandler<TransferMaterialsCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public TransferMaterialsCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        TransferMaterialsCommand request,
        CancellationToken cancellationToken)
    {
        var fromWarehouse = await _db.Warehouses
            .FirstOrDefaultAsync(x => x.Id == request.FromWarehouseId, cancellationToken)
            ?? throw new NotFoundException("Source warehouse not found");

        var toWarehouse = await _db.Warehouses
            .FirstOrDefaultAsync(x => x.Id == request.ToWarehouseId, cancellationToken)
            ?? throw new NotFoundException("Destination warehouse not found");

        var movement = new InventoryMovementEntity(
            InventoryMovementType.Transfer,
            request.FromWarehouseId,
            request.ToWarehouseId,
            null,
            null,
            _currentUser.UserId
        );

        _db.InventoryMovements.Add(movement);

        foreach (var item in request.Items)
        {
            var fromStock = await _db.WarehouseMaterials
                .FirstOrDefaultAsync(
                    x => x.WarehouseId == request.FromWarehouseId &&
                         x.MaterialId == item.MaterialId,
                    cancellationToken)
                ?? throw new DomainApplicationException("Material not found in source warehouse.");

            fromStock.RemoveQty(item.Qty);

            var toStock = await _db.WarehouseMaterials
                .FirstOrDefaultAsync(
                    x => x.WarehouseId == request.ToWarehouseId &&
                         x.MaterialId == item.MaterialId,
                    cancellationToken);

            if (toStock == null)
            {
                toStock = new WarehouseMaterialEntity(
                    request.ToWarehouseId,
                    item.MaterialId,
                    item.Qty);

                _db.WarehouseMaterials.Add(toStock);
            }
            else
            {
                toStock.AddQty(item.Qty);
            }

            var unitCost = await GetUnitCost(item.MaterialId, cancellationToken);

            var movementItem = new InventoryMovementItemEntity(
                movement.Id,
                item.MaterialId,
                item.Qty,
                unitCost);

            _db.InventoryMovementItems.Add(movementItem);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<decimal> GetUnitCost(Guid materialId, CancellationToken cancellationToken)
    {
        var cost = await (
            from i in _db.MaterialPurchaseOrderItems.AsNoTracking()
            join o in _db.MaterialPurchaseOrders.AsNoTracking() on i.PurchaseOrderId equals o.Id
            where i.MaterialId == materialId && o.Status == PurchaseOrderStatus.Received
            orderby o.OrderDate descending
            select (decimal?)i.UnitPrice)
            .FirstOrDefaultAsync(cancellationToken);

        return cost ?? 0m;
    }
}