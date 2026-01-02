using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.Warehouses.UpdateWarehouseMaterialQty;

public sealed class UpdateWarehouseMaterialQtyCommandHandler
    : IRequestHandler<UpdateWarehouseMaterialQtyCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public UpdateWarehouseMaterialQtyCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        UpdateWarehouseMaterialQtyCommand request,
        CancellationToken cancellationToken)
    {
        var warehouseMaterial = await _db.WarehouseMaterials
            .FirstOrDefaultAsync(
                x => x.WarehouseId == request.WarehouseId &&
                     x.MaterialId == request.MaterialId,
                cancellationToken)
            ?? throw new NotFoundException("Material not found in warehouse");

        var delta = Math.Abs(request.Qty - warehouseMaterial.Qty);

        warehouseMaterial.AdjustQty(request.Qty);

        if (delta > 0)
        {
            var unitCost = await GetUnitCost(request.MaterialId, cancellationToken);

            await CreateAdjustmentMovement(
                request.WarehouseId,
                request.MaterialId,
                delta,
                unitCost,
                cancellationToken);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateAdjustmentMovement(
        Guid warehouseId,
        Guid materialId,
        decimal qtyDelta,
        decimal unitCost,
        CancellationToken cancellationToken)
    {
        var movement = new InventoryMovementEntity(
            InventoryMovementType.Adjustment,
            warehouseId,
            null,
            null,
            null,
            _currentUser.UserId);

        _db.InventoryMovements.Add(movement);

        _db.InventoryMovementItems.Add(
            new InventoryMovementItemEntity(
                movement.Id,
                materialId,
                qtyDelta,
                unitCost));

        await Task.CompletedTask;
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