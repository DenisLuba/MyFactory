using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionOrder;

public sealed class UpdateProductionOrderCommandHandler : IRequestHandler<UpdateProductionOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateProductionOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateProductionOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        if (po.Status != ProductionOrderStatus.New)
            throw new DomainApplicationException("Only new production orders can be updated.");
        po.Update(request.DepartmentId, request.QtyPlanned);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
