using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Domain.Tests.Misc;

public class AdvanceReportMiscTests
{
    [Fact]
    public void AddReport_WithEmptyFileId_Throws()
    {
        var advance = new Advance(Guid.NewGuid(), 750m, new DateOnly(2025, 2, 10));
        advance.Issue();

        Assert.Throws<DomainException>(() =>
            advance.AddReport("Stationary", 100m, Guid.Empty, new DateOnly(2025, 2, 11)));
    }

    [Fact]
    public void AddReport_WithValidFileId_PreservesIdentifier()
    {
        var advance = new Advance(Guid.NewGuid(), 1_200m, new DateOnly(2025, 3, 5));
        advance.Issue();
        var fileId = Guid.NewGuid();

        var report = advance.AddReport("Fuel", 300m, fileId, new DateOnly(2025, 3, 7));

        Assert.Equal(fileId, report.FileId);
    }
}
