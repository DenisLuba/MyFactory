using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Domain.Entities.Parties;

public class CustomerEntity : ActivatableEntity
{
    public string Name { get; private set; }

    public IReadOnlyCollection<ContactLinkEntity> ContactLinks { get; private set; } = new List<ContactLinkEntity>();
    public IReadOnlyCollection<SalesOrderEntity> SalesOrders { get; private set; } = new List<SalesOrderEntity>();
    public IReadOnlyCollection<ShipmentEntity> Shipments { get; private set; } = new List<ShipmentEntity>();
    public IReadOnlyCollection<ShipmentReturnEntity> ShipmentReturns { get; private set; } = new List<ShipmentReturnEntity>();

    public CustomerEntity(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }
}

public class ContactEntity : AuditableEntity
{
    public ContactType ContactType { get; private set; }
    public string Value { get; private set; }
    public bool IsPrimary { get; private set; }

    public IReadOnlyCollection<ContactLinkEntity> ContactLinks { get; private set; } = new List<ContactLinkEntity>();

    public ContactEntity(ContactType contactType, string value, bool isPrimary)
    {
        Guard.AgainstNullOrWhiteSpace(value, nameof(value));
        ContactType = contactType;
        Value = value;
        IsPrimary = isPrimary;
    }
}

public enum ContactType
{
    Address,
    Phone,
    Email,
    Telegram,
    Whatsapp,
    Other
}

public class ContactLinkEntity : AuditableEntity
{
    public Guid ContactId { get; private set; }
    public ContactOwnerType OwnerType { get; private set; }
    public Guid OwnerId { get; private set; }

    public ContactLinkEntity(Guid contactId, ContactOwnerType ownerType, Guid ownerId)
    {
        Guard.AgainstEmptyGuid(contactId, nameof(contactId));
        Guard.AgainstEmptyGuid(ownerId, nameof(ownerId));
        ContactId = contactId;
        OwnerType = ownerType;
        OwnerId = ownerId;
    }
}

public enum ContactOwnerType
{
    Customer,
    User,
    Employee
}