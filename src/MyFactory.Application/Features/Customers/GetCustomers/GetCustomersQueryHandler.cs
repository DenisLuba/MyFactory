using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Customers;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Application.Features.Customers.GetCustomers;

public sealed class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IReadOnlyList<CustomerListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetCustomersQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CustomerListItemDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _db.Customers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .GroupJoin(
                _db.ContactLinks.AsNoTracking(),
                customer => customer.Id,
                link => link.OwnerId,
                (customer, links) => new { customer, links }
            )
            .Select(x => new
            {
                x.customer,
                Phone = (
                    from l in x.links
                    join c in _db.Contacts on l.ContactId equals c.Id
                    where l.OwnerType == ContactOwnerType.Customer
                          && c.ContactType == ContactType.Phone
                          && c.IsPrimary
                    select c.Value
                ).FirstOrDefault(),

                Email = (
                    from l in x.links
                    join c in _db.Contacts on l.ContactId equals c.Id
                    where l.OwnerType == ContactOwnerType.Customer
                          && c.ContactType == ContactType.Email
                          && c.IsPrimary
                    select c.Value
                ).FirstOrDefault(),

                Address = (
                    from l in x.links
                    join c in _db.Contacts on l.ContactId equals c.Id
                    where l.OwnerType == ContactOwnerType.Customer
                          && c.ContactType == ContactType.Address
                          && c.IsPrimary
                    select c.Value
                ).FirstOrDefault()
            })
            .OrderBy(x => x.customer.Name)
            .Select(x => new CustomerListItemDto
            {
                Id = x.customer.Id,
                Name = x.customer.Name,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address
            })
            .ToListAsync(cancellationToken);

        return customers;
    }
}
