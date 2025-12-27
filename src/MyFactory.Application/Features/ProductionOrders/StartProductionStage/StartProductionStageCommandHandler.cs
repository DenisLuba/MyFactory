using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.StartProductionStage;

public sealed class StartProductionStageCommandHandler
    : IRequestHandler<StartProductionStageCommand>
{
    private readonly IApplicationDbContext _db;

    public StartProductionStageCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(StartProductionStageCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");

        switch (request.TargetStatus)
        {
            case ProductionOrderStatus.Cutting when po.Status == ProductionOrderStatus.MaterialIssued:
                po.StartCutting();
                break;

            case ProductionOrderStatus.Sewing when po.Status == ProductionOrderStatus.Cutting:
                po.StartSewing();
                break;

            case ProductionOrderStatus.Packaging when po.Status == ProductionOrderStatus.Sewing:
                po.StartPackaging();
                break;

            default:
                throw new DomainException(
                    $"Cannot start stage {request.TargetStatus} from status {po.Status}");
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}