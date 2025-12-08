using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Domain.Tests.Finance;

public class AdvanceTests
{
    [Fact]
    public void AdvanceLifecycle_WithValidData_Works()
    {
        var advance = new Advance(Guid.NewGuid(), 1_000m, new DateOnly(2025, 1, 10), "January travel");

        advance.Approve();
        advance.AddReport("Raw materials", 600m, new DateOnly(2025, 1, 12));
        advance.AddReport("Transport", 400m, new DateOnly(2025, 1, 13));
        advance.Close(new DateOnly(2025, 1, 15));

        Assert.Equal(AdvanceStatus.Closed, advance.Status);
        Assert.Equal(1_000m, advance.ReportedAmount);
        Assert.Equal(0m, advance.RemainingAmount);
        Assert.Equal(2, advance.Reports.Count);
        Assert.Equal(new DateOnly(2025, 1, 15), advance.ClosedAt);
        Assert.Equal("January travel", advance.Description);
    }

    [Fact]
    public void AddReport_ExceedingRemaining_Throws()
    {
        var advance = new Advance(Guid.NewGuid(), 500m, new DateOnly(2025, 1, 5));
        advance.Approve();
        advance.AddReport("Travel", 400m, new DateOnly(2025, 1, 6));

        Assert.Throws<DomainException>(() =>
            advance.AddReport("More", 200m, new DateOnly(2025, 1, 7)));
    }
}
