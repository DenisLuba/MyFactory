using System;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Finance;

public class RevenueReportTests
{
    [Fact]
    public void UpdateSales_AndMarkPaid_Works()
    {
        var report = new RevenueReport(4, 2025, Guid.NewGuid(), 100m, 250m, false, null);

        report.UpdateSales(120m, 260m);
        report.MarkPaid(new DateOnly(2025, 5, 15));

        Assert.Equal(120m, report.Quantity);
        Assert.Equal(260m, report.UnitPrice);
        Assert.Equal(31_200m, report.TotalRevenue);
        Assert.True(report.IsPaid);
        Assert.Equal(new DateOnly(2025, 5, 15), report.PaymentDate);
    }

    [Fact]
    public void Constructor_PaidWithoutDate_Throws()
    {
        Assert.Throws<DomainException>(() =>
            new RevenueReport(1, 2025, Guid.NewGuid(), 10m, 100m, true, null));
    }
}
