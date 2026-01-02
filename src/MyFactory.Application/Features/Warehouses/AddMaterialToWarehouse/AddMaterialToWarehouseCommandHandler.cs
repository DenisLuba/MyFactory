using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace MyFactory.Application.Features.Warehouses.AddMaterialToWarehouse;

public sealed class AddMaterialToWarehouseCommandHandler
    : IRequestHandler<AddMaterialToWarehouseCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AddMaterialToWarehouseCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        AddMaterialToWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        var warehouse = await _db.Warehouses
            .FirstOrDefaultAsync(x => x.Id == request.WarehouseId, cancellationToken)
            ?? throw new NotFoundException("Warehouse not found");

        if (warehouse.Type == WarehouseType.FinishedGoods)
            throw new ValidationException("Cannot add materials to finished goods warehouse");

        var exists = await _db.WarehouseMaterials.AnyAsync(
            x => x.WarehouseId == request.WarehouseId &&
                 x.MaterialId == request.MaterialId,
            cancellationToken);

        if (exists)
            throw new ValidationException("Material already exists in warehouse");

        var warehouseMaterial = new WarehouseMaterialEntity(
            request.WarehouseId,
            request.MaterialId,
            request.Qty);

        _db.WarehouseMaterials.Add(warehouseMaterial);

        var unitCost = await GetUnitCost(request.MaterialId, cancellationToken);

        await CreateAdjustmentMovement(
            request.WarehouseId,
            request.MaterialId,
            request.Qty,
            unitCost,
            cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateAdjustmentMovement(
        Guid warehouseId,
        Guid materialId,
        decimal qty,
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
                qty,
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
