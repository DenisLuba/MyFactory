using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.CompleteSalesOrder;

public sealed class CompleteSalesOrderCommandHandler : IRequestHandler<CompleteSalesOrderCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CompleteSalesOrderCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(CompleteSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.SalesOrders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException("Sales order not found");
        if (order.Status != SalesOrderStatus.Confirmed)
            throw new DomainException("Only confirmed orders can be completed.");
        var orderedQty = await _db.SalesOrderItems.Where(x => x.SalesOrderId == request.OrderId).SumAsync(x => x.QtyOrdered, cancellationToken);
        var shippedQty = await 
        (
            from fgmItem in _db.FinishedGoodsMovementItems
            join fg in _db.FinishedGoods
                on fgmItem.ProductId equals fg.ProductId
            join prodOrder in _db.ProductionOrders
                on fg.ProductionOrderId equals prodOrder.Id
            join soItem in _db.SalesOrderItems
                on prodOrder.SalesOrderItemId equals soItem.Id
            where soItem.SalesOrderId == request.OrderId
            select fgmItem.Qty
        ).SumAsync(cancellationToken);

        if (shippedQty < orderedQty)
            throw new DomainException("Cannot complete order: not all items shipped.");
        order.Fulfill();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
