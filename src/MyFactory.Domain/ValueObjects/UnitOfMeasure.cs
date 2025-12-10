using System.Collections.Generic;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

/// <summary>
/// Represents a normalized unit of measure for quantities and BOM items.
/// </summary>
public sealed class UnitOfMeasure : ValueObject
{
    private const int MaxLength = 16;

    private UnitOfMeasure(string value)
    {
        Guard.AgainstNullOrWhiteSpace(value, "Unit of measure is required.");
        var normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length > MaxLength)
        {
            throw new DomainException($"Unit of measure cannot exceed {MaxLength} characters.");
        }

        Value = normalized;
    }

    public string Value { get; }

    public static UnitOfMeasure From(string value) => new(value);

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(UnitOfMeasure unit) => unit.Value;

    public static explicit operator UnitOfMeasure(string value) => From(value);
}
