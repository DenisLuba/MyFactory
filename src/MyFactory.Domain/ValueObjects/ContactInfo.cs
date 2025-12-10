using System.Collections.Generic;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

/// <summary>
/// Describes a contact person and the way to reach them.
/// </summary>
public sealed class ContactInfo : ValueObject
{
    private const int NameMaxLength = 128;
    private const int DetailsMaxLength = 256;

    private ContactInfo(string? personName, string details)
    {
        Guard.AgainstNullOrWhiteSpace(details, "Contact details are required.");
        var normalizedDetails = details.Trim();
        if (normalizedDetails.Length > DetailsMaxLength)
        {
            throw new DomainException($"Contact details cannot exceed {DetailsMaxLength} characters.");
        }

        if (!string.IsNullOrWhiteSpace(personName) && personName.Trim().Length > NameMaxLength)
        {
            throw new DomainException($"Contact person name cannot exceed {NameMaxLength} characters.");
        }

        PersonName = string.IsNullOrWhiteSpace(personName) ? null : personName.Trim();
        Details = normalizedDetails;
    }

    public string? PersonName { get; }

    public string Details { get; }

    public static ContactInfo From(string details) => new(null, details);

    public static ContactInfo From(string? personName, string details) => new(personName, details);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PersonName?.ToUpperInvariant();
        yield return Details.ToUpperInvariant();
    }
}
