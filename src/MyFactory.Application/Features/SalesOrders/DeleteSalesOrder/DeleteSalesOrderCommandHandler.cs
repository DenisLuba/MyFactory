using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.DeleteSalesOrder;

public sealed class DeleteSalesOrderCommandHandler : IRequestHandler<DeleteSalesOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public DeleteSalesOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeleteSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.SalesOrders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException("Sales order not found");

        if (order.Status != SalesOrderStatus.New)
            throw new DomainException("Only new orders can be deleted.");

        var items = await _db.SalesOrderItems.Where(x => x.SalesOrderId == request.OrderId).ToListAsync(cancellationToken);
        _db.SalesOrderItems.RemoveRange(items);
        _db.SalesOrders.Remove(order);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
