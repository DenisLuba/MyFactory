using System.Collections.Generic;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

public sealed class Period : ValueObject
{
    private Period(int month, int year)
    {
        if (month is < 1 or > 12)
        {
            throw new DomainException("Period month must be between 1 and 12.");
        }

        if (year < 2000)
        {
            throw new DomainException("Period year must be 2000 or later.");
        }

        Month = month;
        Year = year;
    }

    public int Month { get; }

    public int Year { get; }

    public static Period From(int month, int year) => new(month, year);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Month;
        yield return Year;
    }
}
