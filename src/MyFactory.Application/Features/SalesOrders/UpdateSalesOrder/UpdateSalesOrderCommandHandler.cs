using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.UpdateSalesOrder;

public sealed class UpdateSalesOrderCommandHandler : IRequestHandler<UpdateSalesOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateSalesOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.SalesOrders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException("Sales order not found");

        if (order.Status != SalesOrderStatus.New)
            throw new DomainApplicationException("Only new orders can be updated.");

        order.Update(request.CustomerId, request.OrderDate);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
