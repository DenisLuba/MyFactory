using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

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

        if (warehouseMaterial.Qty > 0)
        {
            await CreateAdjustmentMovement(
                request.WarehouseId,
                request.MaterialId,
                -warehouseMaterial.Qty,
                cancellationToken);
        }

        _db.WarehouseMaterials.Remove(warehouseMaterial);

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateAdjustmentMovement(
        Guid warehouseId,
        Guid materialId,
        decimal qtyDelta,
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
                qtyDelta));

        await Task.CompletedTask;
    }
}