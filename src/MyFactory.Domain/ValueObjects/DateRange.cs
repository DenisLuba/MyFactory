using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    private DateRange(DateTime start, DateTime? end)
    {
        Guard.AgainstDefaultDate(start, "Start date is required.");
        if (end.HasValue && end.Value < start)
        {
            throw new DomainException("End date cannot be earlier than start date.");
        }

        Start = start;
        End = end;
    }

    public DateTime Start { get; }

    public DateTime? End { get; }

    public static DateRange From(DateTime start, DateTime? end) => new(start, end);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}
