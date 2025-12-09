using System;

namespace MyFactory.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    protected BaseEntity(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("Entity id cannot be empty.");
        }

        Id = id;
        CreatedAt = DateTime.UtcNow;
    }
}


