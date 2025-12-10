using System.Collections.Generic;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

/// <summary>
/// Standardized description field with consistent trimming and length validation.
/// </summary>
public sealed class Description : ValueObject
{
    public const int MaxLength = 2048;

    private Description(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Value = null;
            return;
        }

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new DomainException($"Description cannot exceed {MaxLength} characters.");
        }

        Value = normalized;
    }

    public string? Value { get; }

    public static Description From(string? value) => new(value);

    public override string ToString() => Value ?? string.Empty;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
