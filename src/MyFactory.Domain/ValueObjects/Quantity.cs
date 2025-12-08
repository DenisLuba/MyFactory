using System.Collections.Generic;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

public sealed class Quantity : ValueObject
{
    private Quantity(decimal value)
    {
        if (value < 0)
        {
            throw new DomainException("Quantity cannot be negative.");
        }

        Value = value;
    }

    public decimal Value { get; }

    public static Quantity From(decimal value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
