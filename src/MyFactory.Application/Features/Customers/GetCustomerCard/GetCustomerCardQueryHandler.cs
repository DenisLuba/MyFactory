using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Parties;
using MyFactory.Application.DTOs.Customers;

namespace MyFactory.Application.Features.Customers.GetCustomerCard;

public sealed class GetCustomerCardQueryHandler(IApplicationDbContext db) : IRequestHandler<GetCustomerCardQuery, CustomerCardDto>
{
    public async Task<CustomerCardDto> Handle(GetCustomerCardQuery request, CancellationToken cancellationToken)
    {
        var customer = await db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken) 
            ?? throw new NotFoundException("Customer not found.");

        var phone = await GetContactValue(customer.Id, ContactType.Phone, cancellationToken);

        var email = await GetContactValue(customer.Id, ContactType.Email, cancellationToken);

        var address = await GetContactValue(customer.Id, ContactType.Address, cancellationToken);

        var orders = await (from o in db.SalesOrders.AsNoTracking()
                            where o.CustomerId == customer.Id
                            orderby o.OrderDate descending
                            select new CustomerSalesOrderDto
                            {
                                Id = o.Id,
                                OrderNumber = o.OrderNumber,
                                OrderDate = o.OrderDate,
                                Status = o.Status
                            }).ToListAsync(cancellationToken);

        return new CustomerCardDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Phone = phone,
            Email = email,
            Address = address,
            Orders = orders
        };
    }

    private Task<string?> GetContactValue(Guid customerId, ContactType contactType, CancellationToken cancellationToken)
    {
        return (
            from l in db.ContactLinks.AsNoTracking()
            join c in db.Contacts.AsNoTracking() on l.ContactId equals c.Id
            where l.OwnerType == ContactOwnerType.Customer
               && l.OwnerId == customerId
               && c.ContactType == contactType
               && c.IsPrimary
            select c.Value
        ).FirstOrDefaultAsync(cancellationToken);
    }
}
