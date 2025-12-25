using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;

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

        var maxNumber = await _db.SalesOrders
            .OrderByDescending(x => x.OrderNumber)
            .Select(x => x.OrderNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;
        if (!string.IsNullOrWhiteSpace(maxNumber) && maxNumber.StartsWith("SO-") && int.TryParse(maxNumber[3..], out var parsed))
            nextNumber = parsed + 1;
        var orderNumber = $"SO-{nextNumber:D4}";

        var order = SalesOrderEntity.Create(
            orderNumber,
            request.CustomerId,
            request.OrderDate,
            _currentUser.UserId
        );

        _db.SalesOrders.Add(order);
        await _db.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}
