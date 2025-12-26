using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Customers;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Application.Features.Customers.GetCustomerDetails;

public sealed class GetCustomerDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, CustomerDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetCustomerDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<CustomerDetailsDto> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.CustomerId && x.IsActive, cancellationToken);
        if (customer is null)
            throw new NotFoundException("Customer not found");

        var phone = await (from link in _db.ContactLinks.AsNoTracking()
                           join contact in _db.Contacts.AsNoTracking() on link.ContactId equals contact.Id
                           where link.OwnerType == ContactOwnerType.Customer && link.OwnerId == customer.Id && contact.ContactType == ContactType.Phone && contact.IsPrimary
                           select contact.Value).FirstOrDefaultAsync(cancellationToken);
        var email = await (from link in _db.ContactLinks.AsNoTracking()
                           join contact in _db.Contacts.AsNoTracking() on link.ContactId equals contact.Id
                           where link.OwnerType == ContactOwnerType.Customer && link.OwnerId == customer.Id && contact.ContactType == ContactType.Email && contact.IsPrimary
                           select contact.Value).FirstOrDefaultAsync(cancellationToken);
        var address = await (from link in _db.ContactLinks.AsNoTracking()
                             join contact in _db.Contacts.AsNoTracking() on link.ContactId equals contact.Id
                             where link.OwnerType == ContactOwnerType.Customer && link.OwnerId == customer.Id && contact.ContactType == ContactType.Address && contact.IsPrimary
                             select contact.Value).FirstOrDefaultAsync(cancellationToken);

        return new CustomerDetailsDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Phone = phone,
            Email = email,
            Address = address
        };
    }
}
