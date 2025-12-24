using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

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

        var delta = request.Qty - warehouseMaterial.Qty;

        warehouseMaterial.AdjustQty(request.Qty);

        if (delta != 0)
        {
            await CreateAdjustmentMovement(
                request.WarehouseId,
                request.MaterialId,
                delta,
                cancellationToken);
        }

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