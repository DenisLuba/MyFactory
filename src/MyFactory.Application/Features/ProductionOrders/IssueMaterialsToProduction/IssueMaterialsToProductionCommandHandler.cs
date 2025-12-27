using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.ProductionOrders.IssueMaterialsToProduction;

public sealed class IssueMaterialsToProductionCommandHandler
    : IRequestHandler<IssueMaterialsToProductionCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public IssueMaterialsToProductionCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        IssueMaterialsToProductionCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Production order
        var po = await _db.ProductionOrders
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");

        if (po.Status != ProductionOrderStatus.New)
            throw new DomainException("Materials can be issued only for NEW production orders.");

        // 2. SalesOrderItem -> Product
        var soi = await _db.SalesOrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == po.SalesOrderItemId, cancellationToken)
            ?? throw new NotFoundException("Sales order item not found");

        // 3. Требуемые материалы по спецификации
        var requiredMaterials = await (
            from pm in _db.ProductMaterials.AsNoTracking()
            where pm.ProductId == soi.ProductId
            select new
            {
                pm.MaterialId,
                RequiredQty = pm.QtyPerUnit * po.QtyPlanned
            }
        ).ToListAsync(cancellationToken);

        // 4. Проверка: выдано ровно столько, сколько требуется
        foreach (var required in requiredMaterials)
        {
            var issuedQty = request.Materials
                .Where(x => x.MaterialId == required.MaterialId)
                .Sum(x => x.Qty);

            if (issuedQty != required.RequiredQty)
            {
                throw new DomainException(
                    $"Invalid issued quantity for material {required.MaterialId}. " +
                    $"Required: {required.RequiredQty}, Issued: {issuedQty}");
            }
        }

        // 5. Проверка остатков по складам
        foreach (var line in request.Materials)
        {
            var stock = await _db.WarehouseMaterials
                .FirstOrDefaultAsync(x =>
                    x.WarehouseId == line.WarehouseId &&
                    x.MaterialId == line.MaterialId,
                    cancellationToken);

            if (stock is null || stock.Qty < line.Qty)
                throw new DomainException("Not enough material in warehouse.");
        }

        // 6. Группируем по складам -> одно перемещение = один склад
        var materialsByWarehouse = request.Materials
            .GroupBy(x => x.WarehouseId);

        foreach (var warehouseGroup in materialsByWarehouse)
        {
            var warehouseId = warehouseGroup.Key;

            // 6.1 Inventory movement (один склад -> один цех)
            var movement = new InventoryMovementEntity(
                InventoryMovementType.IssueToDept,
                fromWarehouseId: warehouseId,
                toWarehouseId: null,
                toDepartmentId: po.DepartmentId,
                productionOrderId: po.Id,
                createdBy: _currentUser.UserId);

            _db.InventoryMovements.Add(movement);

            // 6.2 Строки движения + списание остатков
            foreach (var line in warehouseGroup)
            {
                var stock = await _db.WarehouseMaterials
                    .FirstAsync(x =>
                        x.WarehouseId == line.WarehouseId &&
                        x.MaterialId == line.MaterialId,
                        cancellationToken);

                stock.RemoveQty(line.Qty);

                var item = new InventoryMovementItemEntity(
                    movement.Id,
                    line.MaterialId,
                    line.Qty);

                _db.InventoryMovementItems.Add(item);
            }
        }

        // 7. Смена статуса PO
        po.IssueMaterials();

        await _db.SaveChangesAsync(cancellationToken);
    }
}