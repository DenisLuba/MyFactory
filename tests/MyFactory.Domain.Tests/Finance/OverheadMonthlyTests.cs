using System;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Finance;

public class OverheadMonthlyTests
{
    [Fact]
    public void Constructor_WithValidData_SetsValues()
    {
        var expenseTypeId = Guid.NewGuid();
        var overhead = new OverheadMonthly(3, 2025, expenseTypeId, 50_000m, "March utilities");

        Assert.Equal(3, overhead.PeriodMonth);
        Assert.Equal(2025, overhead.PeriodYear);
        Assert.Equal(expenseTypeId, overhead.ExpenseTypeId);
        Assert.Equal(50_000m, overhead.Amount);
        Assert.Equal("March utilities", overhead.Notes);
    }

    [Fact]
    public void Constructor_WithInvalidMonth_Throws()
    {
        Assert.Throws<DomainException>(() => new OverheadMonthly(13, 2025, Guid.NewGuid(), 10_000m, null));
    }
}
