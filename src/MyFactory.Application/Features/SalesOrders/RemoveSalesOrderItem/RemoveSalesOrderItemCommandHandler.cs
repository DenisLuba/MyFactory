using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.RemoveSalesOrderItem;

public sealed class RemoveSalesOrderItemCommandHandler : IRequestHandler<RemoveSalesOrderItemCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveSalesOrderItemCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RemoveSalesOrderItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _db.SalesOrderItems.FirstOrDefaultAsync(x => x.Id == request.OrderItemId, cancellationToken)
            ?? throw new NotFoundException("Order item not found");
        var order = await _db.SalesOrders.FirstOrDefaultAsync(x => x.Id == item.SalesOrderId, cancellationToken)
            ?? throw new NotFoundException("Sales order not found");
        if (order.Status != SalesOrderStatus.New)
            throw new DomainException("Only new orders can be modified.");
        _db.SalesOrderItems.Remove(item);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
