using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Finance;

public class MonthlyProfitTests
{
    [Fact]
    public void Constructor_WithValidData_ComputesProfit()
    {
        var profit = new MonthlyProfit(6, 2025, 1_000_000m, 600_000m, 150_000m);

        Assert.Equal(250_000m, profit.Profit);
    }

    [Fact]
    public void Constructor_WithNegativeRevenue_Throws()
    {
        Assert.Throws<DomainException>(() => new MonthlyProfit(6, 2025, -1m, 100m, 50m));
    }
}
