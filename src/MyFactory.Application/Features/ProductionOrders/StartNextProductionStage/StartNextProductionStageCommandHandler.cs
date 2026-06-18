using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.StartNextProductionStage;

public sealed class StartNextProductionStageCommandHandler
    : IRequestHandler<StartNextProductionStageCommand>
{
    private readonly IApplicationDbContext _db;

    public StartNextProductionStageCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(StartNextProductionStageCommand request, CancellationToken cancellationToken)
    {
        var productionOrder = await _db.ProductionOrders
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found.");

        switch (productionOrder.Status)
        {
            case ProductionOrderStatus.MaterialIssued:
                if (productionOrder.QtyPlanned <= 0)
                    throw new DomainApplicationException("Cannot start cutting if the planned amount is less than or equal to zero");

                await EnsureAllMaterialsIssuedAsync(productionOrder, cancellationToken);

                productionOrder.StartCutting();
                break;

            case ProductionOrderStatus.Cutting:
                if (productionOrder.QtyCut <= 0)
                    throw new DomainApplicationException("Cannot start sewing if not a single piece is cut.");

                productionOrder.StartSewing();
                break;

            case ProductionOrderStatus.Sewing:
                if (productionOrder.QtySewn <= 0)
                    throw new DomainApplicationException("Cannot start packaging if not a single piece is sewn.");

                productionOrder.StartPackaging();
                break;

            case ProductionOrderStatus.Packaging:
                if (productionOrder.QtyPacked < productionOrder.QtyPlanned)
                    throw new DomainApplicationException("Cannot complete packaging before the full planned quantity is packed.");

                var qtyToFinish = productionOrder.QtyPacked - productionOrder.QtyFinished;
                if (qtyToFinish > 0)
                    productionOrder.AddFinished(qtyToFinish);

                productionOrder.FinishOrder();
                break;

            default:
                throw new DomainApplicationException("Current production order status cannot be completed.");
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureAllMaterialsIssuedAsync(
        ProductionOrderEntity productionOrder,
        CancellationToken cancellationToken)
    {
        var salesOrderItem = await _db.SalesOrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == productionOrder.SalesOrderItemId, cancellationToken)
            ?? throw new NotFoundException("Sales order item not found.");

        var requiredMaterials = await _db.ProductMaterials
            .AsNoTracking()
            .Where(x => x.ProductId == salesOrderItem.ProductId)
            .Select(x => new
            {
                x.MaterialId,
                RequiredQty = x.QtyPerUnit * productionOrder.QtyPlanned
            })
            .ToListAsync(cancellationToken);

        var issuedMaterials = await (
            from item in _db.InventoryMovementItems.AsNoTracking()
            join movement in _db.InventoryMovements.AsNoTracking()
                on item.MovementId equals movement.Id
            where movement.ProductionOrderId == productionOrder.Id
               && (movement.MovementType == InventoryMovementType.IssueToDept
                   || movement.MovementType == InventoryMovementType.ReturnFromDept)
            group new { item, movement } by item.MaterialId into g
            select new
            {
                MaterialId = g.Key,
                IssuedQty = g.Sum(x =>
                    x.movement.MovementType == InventoryMovementType.ReturnFromDept
                        ? -x.item.Qty
                        : x.item.Qty)
            })
            .ToDictionaryAsync(x => x.MaterialId, x => x.IssuedQty, cancellationToken);

        const decimal epsilon = 0.0001m;

        var missingMaterial = requiredMaterials.FirstOrDefault(required =>
        {
            var issuedQty = issuedMaterials.GetValueOrDefault(required.MaterialId);
            return Math.Abs(issuedQty - required.RequiredQty) > epsilon;
        });

        if (missingMaterial is not null)
        {
            var issuedQty = issuedMaterials.GetValueOrDefault(missingMaterial.MaterialId);

            throw new DomainApplicationException(
                $"Cannot start cutting because not all required materials have been issued. " +
                $"MaterialId: {missingMaterial.MaterialId}. Required: {missingMaterial.RequiredQty}, Issued: {issuedQty}.");
        }
    }
}
