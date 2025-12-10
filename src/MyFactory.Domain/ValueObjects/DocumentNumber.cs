using System.Collections.Generic;
using System.Text.RegularExpressions;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

/// <summary>
/// Strongly typed wrapper around document identifiers such as order numbers or receipt numbers.
/// </summary>
public sealed class DocumentNumber : ValueObject
{
    private const int MaxLength = 64;
    private static readonly Regex FormatRegex = new("^[A-Z0-9_-]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private DocumentNumber(string value)
    {
        Guard.AgainstNullOrWhiteSpace(value, "Document number is required.");
        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new DomainException($"Document number cannot exceed {MaxLength} characters.");
        }

        if (!FormatRegex.IsMatch(normalized))
        {
            throw new DomainException("Document number can contain only letters, digits, '-' or '_'.");
        }

        Value = normalized;
    }

    public string Value { get; }

    public static DocumentNumber From(string value) => new(value);

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(DocumentNumber number) => number.Value;

    public static explicit operator DocumentNumber(string value) => From(value);
}
