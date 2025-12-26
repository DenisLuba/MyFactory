using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Customers;
using MyFactory.Application.DTOs.SalesOrders;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrderDetails;

public sealed class GetSalesOrderDetailsQueryHandler : IRequestHandler<GetSalesOrderDetailsQuery, SalesOrderDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetSalesOrderDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<SalesOrderDetailsDto> Handle(GetSalesOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var order = await (
            from o in _db.SalesOrders.AsNoTracking()
            join c in _db.Customers.AsNoTracking() on o.CustomerId equals c.Id
            where o.Id == request.OrderId
            select new
            {
                Order = o,
                Customer = c
            }
        ).FirstOrDefaultAsync(cancellationToken);

        if (order is null)
            throw new NotFoundException("Sales order not found");

        var customerId = order.Customer.Id;

        var customerDetails = await (
            from customer in _db.Customers.AsNoTracking()
            where customer.Id == customerId && customer.IsActive
            select new CustomerDetailsDto
            {
                Id = customer.Id,
                Name = customer.Name,

                Phone = (
                    from l in _db.ContactLinks.AsNoTracking()
                    join c in _db.Contacts.AsNoTracking() on l.ContactId equals c.Id
                    where l.OwnerType == ContactOwnerType.Customer
                          && l.OwnerId == customer.Id
                          && c.ContactType == ContactType.Phone
                          && c.IsPrimary
                    select c.Value
                ).FirstOrDefault(),

                Email = (
                    from l in _db.ContactLinks.AsNoTracking()
                    join c in _db.Contacts.AsNoTracking() on l.ContactId equals c.Id
                    where l.OwnerType == ContactOwnerType.Customer
                          && l.OwnerId == customer.Id
                          && c.ContactType == ContactType.Email
                          && c.IsPrimary
                    select c.Value
                ).FirstOrDefault(),

                Address = (
                    from l in _db.ContactLinks.AsNoTracking()
                    join c in _db.Contacts.AsNoTracking() on l.ContactId equals c.Id
                    where l.OwnerType == ContactOwnerType.Customer
                          && l.OwnerId == customer.Id
                          && c.ContactType == ContactType.Address
                          && c.IsPrimary
                    select c.Value
                ).FirstOrDefault()
            }
        ).FirstAsync(cancellationToken);

        var items = await (
            from i in _db.SalesOrderItems.AsNoTracking()
            join p in _db.Products.AsNoTracking() on i.ProductId equals p.Id
            where i.SalesOrderId == request.OrderId
            select new SalesOrderItemDto
            {
                Id = i.Id,
                ProductId = p.Id,
                ProductName = p.Name,
                QtyOrdered = i.QtyOrdered
            }
        ).ToListAsync(cancellationToken);

        return new SalesOrderDetailsDto
        {
            Id = order.Order.Id,
            OrderNumber = order.Order.OrderNumber,
            OrderDate = order.Order.OrderDate,
            Status = order.Order.Status,
            Customer = customerDetails,
            Items = items
        };
    }
}
