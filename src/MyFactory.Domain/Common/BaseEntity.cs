using System;

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
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
}


