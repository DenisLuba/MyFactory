using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    private DateRange(DateOnly start, DateOnly? end)
    {
        Guard.AgainstDefaultDate(start, "Start date is required.");
        if (end.HasValue && end.Value < start)
        {
            throw new DomainException("End date cannot be earlier than start date.");
        }

        Start = start;
        End = end;
    }

    public DateOnly Start { get; }

    public DateOnly? End { get; }

    public static DateRange From(DateOnly start, DateOnly? end) => new(start, end);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}
