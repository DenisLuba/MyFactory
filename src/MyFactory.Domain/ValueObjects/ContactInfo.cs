using System.Collections.Generic;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

/// <summary>
/// Encapsulates contact information (phone, email, etc.) with trimming and length validation.
/// </summary>
public sealed class ContactInfo : ValueObject
{
    private const int MaxLength = 256;

    private ContactInfo(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Contact info is required.");
        }

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new DomainException($"Contact info cannot exceed {MaxLength} characters.");
        }

        Value = normalized;
    }

    public string Value { get; }

    public static ContactInfo From(string value) => new(value);

    public static implicit operator string(ContactInfo info) => info.Value;

    public static implicit operator ContactInfo(string value) => From(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
