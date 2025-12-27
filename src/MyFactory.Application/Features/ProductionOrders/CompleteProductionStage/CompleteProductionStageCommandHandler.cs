using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.CompleteProductionStage;

public sealed class CompleteProductionStageCommandHandler : IRequestHandler<CompleteProductionStageCommand>
{
    private readonly IApplicationDbContext _db;

    public CompleteProductionStageCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(CompleteProductionStageCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        switch (po.Status)
        {
            case ProductionOrderStatus.Cutting:
                po.StartCutting();
                break;
            case ProductionOrderStatus.Sewing:
                po.StartSewing();
                break;
            case ProductionOrderStatus.Packaging:
                po.AddPacked(request.Qty);
                po.StartPackaging();
                break;
            default:
                throw new DomainException("Invalid stage for completion.");
        }
        await _db.SaveChangesAsync(cancellationToken);
    }
}
