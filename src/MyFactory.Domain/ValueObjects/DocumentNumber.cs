using System.Collections.Generic;
using System.Text.RegularExpressions;
using MyFactory.Domain.Common;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Domain.ValueObjects;

/// <summary>
/// Strongly-typed representation of business document numbers.
/// Guarantees trimming, max-length enforcement, and allowed characters (letters, digits, '-', '_', '/').
/// </summary>
public sealed class DocumentNumber : ValueObject
{
    private const int MaxLength = 64;
    private static readonly Regex Pattern = new(@"^[A-Za-z0-9\-_/]+$", RegexOptions.Compiled);

    private DocumentNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Document number is required.");
        }

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new DomainException($"Document number cannot exceed {MaxLength} characters.");
        }

        if (!Pattern.IsMatch(normalized))
        {
            throw new DomainException("Document number contains invalid characters.");
        }

        Value = normalized;
    }

    public string Value { get; }

    public static DocumentNumber From(string value) => new(value);

    public static implicit operator string(DocumentNumber number) => number.Value;

    public static implicit operator DocumentNumber(string value) => From(value);

    public static bool operator ==(DocumentNumber? left, DocumentNumber? right) => Equals(left, right);

    public static bool operator !=(DocumentNumber? left, DocumentNumber? right) => !Equals(left, right);

    public override bool Equals(object? obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
