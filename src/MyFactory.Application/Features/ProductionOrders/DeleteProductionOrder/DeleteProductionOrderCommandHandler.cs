using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.DeleteProductionOrder;

public sealed class DeleteProductionOrderCommandHandler : IRequestHandler<DeleteProductionOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public DeleteProductionOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeleteProductionOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        if (po.Status != ProductionOrderStatus.New)
            throw new DomainException("Only new production orders can be deleted.");
        _db.ProductionOrders.Remove(po);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
