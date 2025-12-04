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
        if (id == Guid.Empty)
        {
            throw new DomainException("Entity id cannot be empty.");
        }

        Id = id;
    }
}


