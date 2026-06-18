using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;
using System.Diagnostics;

namespace MyFactory.Application.Features.SalesOrders.CreatSalesOrder;

public sealed class CreateSalesOrderCommandHandler : IRequestHandler<CreateSalesOrderCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateSalesOrderCommandHandler(IApplicationDbContext db, ICurrentUserService currentUserService)
    {
        _db = db;
        _currentUser = currentUserService;
    }

    public async Task<Guid> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null)
            throw new NotFoundException("Customer not found");

        var order = SalesOrderEntity.Create(
            request.CustomerId,
            request.OrderDate,
            _currentUser.UserId
        );

        _db.SalesOrders.Add(order);
        
        await _db.SaveChangesAsync(cancellationToken); 

        return order.Id;
    }
}
