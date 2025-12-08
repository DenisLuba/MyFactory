using System.Collections.Generic;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    private Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new DomainException("Money amount cannot be negative.");
        }

        Amount = amount;
    }

    public decimal Amount { get; }

    public static Money From(decimal amount) => new(amount);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
    }
}
