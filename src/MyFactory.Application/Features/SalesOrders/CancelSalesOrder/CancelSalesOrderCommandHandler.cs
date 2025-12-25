using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.CancelSalesOrder;

public sealed class CancelSalesOrderCommandHandler : IRequestHandler<CancelSalesOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public CancelSalesOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(CancelSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.SalesOrders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException("Sales order not found");
        order.Cancel();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
