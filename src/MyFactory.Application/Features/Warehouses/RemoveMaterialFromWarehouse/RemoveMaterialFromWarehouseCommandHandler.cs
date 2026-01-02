using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.Warehouses.RemoveMaterialFromWarehouse;

public sealed class RemoveMaterialFromWarehouseCommandHandler
    : IRequestHandler<RemoveMaterialFromWarehouseCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public RemoveMaterialFromWarehouseCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        RemoveMaterialFromWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        var warehouseMaterial = await _db.WarehouseMaterials
            .FirstOrDefaultAsync(
                x => x.WarehouseId == request.WarehouseId &&
                     x.MaterialId == request.MaterialId,
                cancellationToken)
            ?? throw new NotFoundException("Material not found in warehouse");

        var unitCost = await GetUnitCost(request.MaterialId, cancellationToken);

        if (warehouseMaterial.Qty > 0)
        {
            var movement = new InventoryMovementEntity(
                InventoryMovementType.Adjustment,
                request.WarehouseId,
                null,
                null,
                null,
                _currentUser.UserId);

            _db.InventoryMovements.Add(movement);

            _db.InventoryMovementItems.Add(
                new InventoryMovementItemEntity(
                    movement.Id,
                    request.MaterialId,
                    warehouseMaterial.Qty,
                    unitCost));
        }

        _db.WarehouseMaterials.Remove(warehouseMaterial);

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