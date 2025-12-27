using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.RemoveProductionStageEmployee;

public sealed class RemoveProductionStageEmployeeCommandHandler : IRequestHandler<RemoveProductionStageEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveProductionStageEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RemoveProductionStageEmployeeCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        if (po.Status != request.Stage)
            throw new DomainException("Stage does not match production order status.");

        if (request.Stage == ProductionOrderStatus.Cutting)
        {
            var op = await _db.CuttingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Cutting operation not found");
            _db.CuttingOperations.Remove(op);
            po.RemoveCut(op.QtyCut);
        }
        else if (request.Stage == ProductionOrderStatus.Sewing)
        {
            var op = await _db.SewingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Sewing operation not found");
            _db.SewingOperations.Remove(op);
            po.RemoveSewn(op.QtySewn);
        }
        else if (request.Stage == ProductionOrderStatus.Packaging)
        {
            var op = await _db.PackagingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Packaging operation not found");
            _db.PackagingOperations.Remove(op);
            po.RemovePacked(op.QtyPacked);
        }
        else
        {
            throw new NotFoundException("Invalid stage for operation removal.");
        }
        await _db.SaveChangesAsync(cancellationToken);
    }
}
