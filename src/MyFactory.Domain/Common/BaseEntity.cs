using MyFactory.Domain.Common;
using MyFactory.Domain.Exceptions;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    protected BaseEntity(Guid id)
    {
        Guard.AgainstEmptyGuid(id, "Entity id cannot be empty.");
        Id = id;
    }

    private readonly List<object> _domainEvents = new();

    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(object @event)
    {
        _domainEvents.Add(@event);
    }

    protected void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}

public abstract class ActivatableEntity : AuditableEntity
{
    public bool IsActive { get; protected set; }

    protected ActivatableEntity(bool isActive = true)
    {
        IsActive = isActive;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException("Entity is already active.");

        IsActive = true;
        Touch();
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("Entity is already inactive.");

        IsActive = false;
        Touch();
    }
}


