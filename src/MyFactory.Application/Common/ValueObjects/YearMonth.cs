namespace MyFactory.Application.Common.ValueObjects;

public readonly record struct YearMonth(int Year, int Month)
{
    public DateOnly Start => new(Year, Month, 1);
    public DateOnly End => Start.AddMonths(1).AddDays(-1);
}