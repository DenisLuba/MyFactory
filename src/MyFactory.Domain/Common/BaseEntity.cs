using System;
using System.Collections.Generic;

namespace MyFactory.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    protected BaseEntity(Guid id)
    {
        Id = id == Guid.Empty
            ? throw new DomainException("Entity id cannot be empty.")
            : id;
    }

    private List<object>? _domainEvents;

    // Return a consistent IReadOnlyCollection<object> without ambiguous ?? types
    public IReadOnlyCollection<object> DomainEvents => _domainEvents is null ? Array.Empty<object>() : _domainEvents.AsReadOnly();

    protected void AddDomainEvent(object @event)
    {
        _domainEvents ??= new List<object>();
        _domainEvents.Add(@event);
    }

    protected void RemoveDomainEvent(object @event)
    {
        _domainEvents?.Remove(@event);
    }

    protected void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
}


