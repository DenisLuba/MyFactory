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

        var salesOrderItem = await _db.SalesOrderItems.FirstOrDefaultAsync(x => x.Id == po.SalesOrderItemId, cancellationToken) 
            ?? throw new NotFoundException("Sales order item not found");

        var allocatedExcludingCurrent = salesOrderItem.QtyAllocated - po.QtyPlanned;
        var remainingForThisOrder = salesOrderItem.QtyOrdered - allocatedExcludingCurrent;

        if (request.QtyPlanned > remainingForThisOrder)
            throw new DomainApplicationException("QtyPlanned cannot exceed remaining ordered quantity.");

        po.Update(request.DepartmentId, request.QtyPlanned);
        salesOrderItem.UpdateQtyAllocated(allocatedExcludingCurrent + request.QtyPlanned);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
