using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;

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
        var normalizedLines = request.Materials
            .GroupBy(x => new { x.MaterialId, x.WarehouseId })
            .Select(g => new
            {
                g.Key.MaterialId,
                g.Key.WarehouseId,
                Qty = g.Sum(x => x.Qty)
            })
            .ToList();

        // 1. Production order
        var po = await _db.ProductionOrders
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");

        if (po.Status != ProductionOrderStatus.New)
            throw new DomainApplicationException("Materials can be issued only for NEW production orders.");

        // 2. SalesOrderItem -> Product
        var soi = await _db.SalesOrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == po.SalesOrderItemId, cancellationToken)
            ?? throw new NotFoundException("Sales order item not found");

        // 3. Проверка состава материалов
        var requiredMaterials = await (
            from pm in _db.ProductMaterials.AsNoTracking()
            where pm.ProductId == soi.ProductId
            select new
            {
                pm.MaterialId,
                RequiredQty = pm.QtyPerUnit * po.QtyPlanned
            }
        ).ToListAsync(cancellationToken);

        // 4. Проверка: в запросе нет материалов вне спецификации
        var requiredMaterialIds = requiredMaterials
            .Select(x => x.MaterialId)
            .ToHashSet();

        var invalidMaterialIds = normalizedLines
            .Select(x => x.MaterialId)
            .Where(x => !requiredMaterialIds.Contains(x))
            .Distinct()
            .ToList();

        if (invalidMaterialIds.Count > 0)
            throw new DomainApplicationException("Request contains materials that are not in the product specification.");

        // 5. Проверка: передали все материалы и ровно в нужном количестве
        foreach (var required in requiredMaterials)
        {
            var issuedQty = normalizedLines
                .Where(x => x.MaterialId == required.MaterialId)
                .Sum(x => x.Qty);

            const decimal epsilon = 0.0001m;
            if (Math.Abs(issuedQty - required.RequiredQty) > epsilon)
            {
                throw new DomainApplicationException(
                    $"Invalid issued quantity for material {required.MaterialId}. " +
                    $"Required: {required.RequiredQty}, Issued: {issuedQty}");
            }
        }

        // 6. Проверка наличия на складах (по общему остатку)
        foreach (var line in normalizedLines)
        {
            var stock = await _db.WarehouseMaterials
                .FirstOrDefaultAsync(x =>
                    x.WarehouseId == line.WarehouseId &&
                    x.MaterialId == line.MaterialId,
                    cancellationToken);

            if (stock is null || stock.Qty < line.Qty)
                throw new DomainApplicationException("Not enough material in warehouse.");
        }

        // 7. Списание и движения
        var materialsByWarehouse = normalizedLines
            .GroupBy(x => x.WarehouseId);

        foreach (var warehouseGroup in materialsByWarehouse)
        {
            var warehouseId = warehouseGroup.Key;

            var movement = new InventoryMovementEntity(
                InventoryMovementType.IssueToDept,
                fromWarehouseId: warehouseId,
                toWarehouseId: null,
                toDepartmentId: po.DepartmentId,
                productionOrderId: po.Id,
                createdBy: _currentUser.UserId);

            _db.InventoryMovements.Add(movement);

            foreach (var line in warehouseGroup)
            {
                var stock = await _db.WarehouseMaterials
                    .FirstAsync(x =>
                        x.WarehouseId == line.WarehouseId &&
                        x.MaterialId == line.MaterialId,
                        cancellationToken);

                stock.RemoveQty(line.Qty);

                var unitCost = await GetUnitCost(line.MaterialId, cancellationToken);

                var item = new InventoryMovementItemEntity(
                    movement.Id,
                    line.MaterialId,
                    line.Qty,
                    unitCost);

                _db.InventoryMovementItems.Add(item);
            }
        }

        // 8. Закрываем стадию PO
        po.IssueMaterials();

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
