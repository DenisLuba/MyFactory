using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.CancelProductionOrder;

public sealed class CancelProductionOrderCommandHandler : IRequestHandler<CancelProductionOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public CancelProductionOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(CancelProductionOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        if (po.Status == ProductionOrderStatus.Finished)
            throw new DomainException("Cannot cancel a finished order.");
        po.CancelOrder();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
