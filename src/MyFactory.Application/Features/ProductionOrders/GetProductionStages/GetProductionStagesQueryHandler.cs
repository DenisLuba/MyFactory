using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStages;

public sealed class GetProductionStagesQueryHandler : IRequestHandler<GetProductionStagesQuery, IReadOnlyList<ProductionStageSummaryDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductionStagesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductionStageSummaryDto>> Handle(GetProductionStagesQuery request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        var result = new List<ProductionStageSummaryDto>
        {
            new()
            {
                Stage = ProductionOrderStatus.Cutting,
                CompletedQty = po.QtyCut,
                RemainingQty = Math.Max(0, po.QtyPlanned - po.QtyCut)
            },
            new()
            {
                Stage = ProductionOrderStatus.Sewing,
                CompletedQty = po.QtySewn,
                RemainingQty = po.QtyPlanned - po.QtySewn
            },
            new()
            {
                Stage = ProductionOrderStatus.Packaging,
                CompletedQty = po.QtyPacked,
                RemainingQty = po.QtyPlanned - po.QtyPacked
            }
        };
        return result;
    }
}
