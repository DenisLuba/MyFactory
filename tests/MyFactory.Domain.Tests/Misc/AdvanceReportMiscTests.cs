using System;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Misc;

public class AdvanceReportMiscTests
{
    [Fact]
    public void AddReport_WithDefaultReportedDate_Throws()
    {
        var advance = new Advance(Guid.NewGuid(), 750m, new DateOnly(2025, 2, 10));
        advance.Approve();

        Assert.Throws<DomainException>(() =>
            advance.AddReport("Stationary", 100m, default, Guid.NewGuid(), new DateOnly(2025, 2, 9)));
    }

    [Fact]
    public void AddReport_WithValidData_PreservesReportedDate()
    {
        var advance = new Advance(Guid.NewGuid(), 1_200m, new DateOnly(2025, 3, 5));
        advance.Approve();
        var reportedAt = new DateOnly(2025, 3, 7);

        var report = advance.AddReport("Fuel", 300m, reportedAt, Guid.NewGuid(), new DateOnly(2025, 3, 7));

        Assert.Equal(reportedAt, report.ReportedAt);
    }

    [Fact]
    public void AddReport_WithSpentDateAfterReported_Throws()
    {
        var advance = new Advance(Guid.NewGuid(), 900m, new DateOnly(2025, 4, 1));
        advance.Approve();

        Assert.Throws<DomainException>(() =>
            advance.AddReport("Late receipt", 100m, new DateOnly(2025, 4, 2), Guid.NewGuid(), new DateOnly(2025, 4, 3)));
    }
}
