using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.CreateProductionOrder;

public sealed class CreateProductionOrderCommandHandler : IRequestHandler<CreateProductionOrderCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateProductionOrderCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateProductionOrderCommand request, CancellationToken cancellationToken)
    {
		var salesOrderItem = await _db.SalesOrderItems.FirstOrDefaultAsync(x => x.Id == request.SalesOrderItemId, cancellationToken) 
            ?? throw new NotFoundException("Sales order item not found");

        var remainingQty = salesOrderItem.QtyOrdered - salesOrderItem.QtyAllocated;
        if (request.QtyPlanned > remainingQty)
            throw new DomainApplicationException("QtyPlanned cannot exceed remaining ordered quantity.");

		var entity = new ProductionOrderEntity(
            request.SalesOrderItemId,
            request.DepartmentId,
            request.QtyPlanned,
            _currentUser.UserId
        );

        salesOrderItem.UpdateQtyAllocated(salesOrderItem.QtyAllocated + request.QtyPlanned);

        _db.ProductionOrders.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
