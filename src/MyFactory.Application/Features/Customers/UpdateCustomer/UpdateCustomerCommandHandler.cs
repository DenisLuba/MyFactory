using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Application.Features.Customers.UpdateCustomer;

public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateCustomerCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId && x.IsActive, cancellationToken)
            ?? throw new NotFoundException("Customer not found");
        customer.Update(request.Name);

        await UpdateContact(ContactType.Phone, request.Phone, customer.Id, cancellationToken);
        await UpdateContact(ContactType.Email, request.Email, customer.Id, cancellationToken);
        await UpdateContact(ContactType.Address, request.Address, customer.Id, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateContact(ContactType type, string? value, Guid customerId, CancellationToken cancellationToken)
    {
        var existing = await (
            from l in _db.ContactLinks
            join c in _db.Contacts on l.ContactId equals c.Id
            where l.OwnerType == ContactOwnerType.Customer
                  && l.OwnerId == customerId
                  && c.ContactType == type
                  && c.IsPrimary
            select new { Link = l, Contact = c }
        ).FirstOrDefaultAsync(cancellationToken);

        // удалить
        if (string.IsNullOrWhiteSpace(value))
        {
            if (existing is not null)
                _db.ContactLinks.Remove(existing.Link);

            return;
        }

        // создать
        if (existing is null)
        {
            var contact = ContactEntity.Create(type, value, true);
            _db.Contacts.Add(contact);
            _db.ContactLinks.Add(new ContactLinkEntity(contact.Id, ContactOwnerType.Customer, customerId));
            return;
        }

        // обновить
        if (existing.Contact.Value != value)
        {
            existing.Contact.UpdateValue(value);
        }
    }
}