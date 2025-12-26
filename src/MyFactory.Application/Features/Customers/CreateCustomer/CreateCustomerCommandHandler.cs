using MediatR;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Application.Features.Customers.CreateCustomer;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateCustomerCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = CustomerEntity.Create(request.Name);
        _db.Customers.Add(customer);


        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            var phone = ContactEntity.Create(ContactType.Phone, request.Phone, true);
            _db.Contacts.Add(phone);
            _db.ContactLinks.Add(ContactLinkEntity.Create(phone.Id, ContactOwnerType.Customer, customer.Id));
        }
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var email = ContactEntity.Create(ContactType.Email, request.Email, true);
            _db.Contacts.Add(email);
            _db.ContactLinks.Add(ContactLinkEntity.Create(email.Id, ContactOwnerType.Customer, customer.Id));
        }
        if (!string.IsNullOrWhiteSpace(request.Address))
        {
            var address = ContactEntity.Create(ContactType.Address, request.Address, true);
            _db.Contacts.Add(address);
            _db.ContactLinks.Add(ContactLinkEntity.Create(address.Id, ContactOwnerType.Customer, customer.Id));
        }

        await _db.SaveChangesAsync(cancellationToken);
        return customer.Id;
    }
}
