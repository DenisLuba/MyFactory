using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.StartSalesOrder;

public sealed class StartSalesOrderCommandHandler : IRequestHandler<StartSalesOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public StartSalesOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(StartSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.SalesOrders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException("Sales order not found");
        if (order.Status != SalesOrderStatus.New)
            throw new DomainException("Only new orders can be started.");
        var hasItems = await _db.SalesOrderItems.AnyAsync(x => x.SalesOrderId == request.OrderId, cancellationToken);
        if (!hasItems)
            throw new DomainException("Order must have at least one item to start.");
        order.Confirm();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
