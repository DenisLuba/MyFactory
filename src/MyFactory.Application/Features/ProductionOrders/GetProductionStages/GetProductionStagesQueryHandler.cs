using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStages;

public sealed class GetProductionStagesQueryHandler
    : IRequestHandler<GetProductionStagesQuery, IReadOnlyList<ProductionStageSummaryDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductionStagesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductionStageSummaryDto>> Handle(
        GetProductionStagesQuery request,
        CancellationToken cancellationToken)
    {
        var productionOrder = await _db.ProductionOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found.");

        return
        [
            new ProductionStageSummaryDto
            {
                Stage = ProductionOrderStatus.Cutting,
                CompletedQty = productionOrder.QtyCut,
                RemainingQty = Math.Max(0, productionOrder.QtyPlanned - productionOrder.QtyCut)
            },
            new ProductionStageSummaryDto
            {
                Stage = ProductionOrderStatus.Sewing,
                CompletedQty = productionOrder.QtySewn,
                RemainingQty = Math.Max(0, productionOrder.QtyCut - productionOrder.QtySewn)
            },
            new ProductionStageSummaryDto
            {
                Stage = ProductionOrderStatus.Packaging,
                CompletedQty = productionOrder.QtyPacked,
                RemainingQty = Math.Max(0, productionOrder.QtySewn - productionOrder.QtyPacked)
            }
        ];
    }
}

