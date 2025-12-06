using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Domain.Tests.Finance;

public class ProductionCostFactTests
{
    [Fact]
    public void Constructor_WithValidData_ComputesTotal()
    {
        var fact = new ProductionCostFact(2, 2025, Guid.NewGuid(), 1_000m, 200_000m, 150_000m, 50_000m);

        Assert.Equal(400_000m, fact.TotalCost);
        Assert.Equal(1_000m, fact.QuantityProduced);
    }

    [Fact]
    public void UpdateCosts_WithNegativeValue_Throws()
    {
        var fact = new ProductionCostFact(2, 2025, Guid.NewGuid(), 1_000m, 200_000m, 150_000m, 50_000m);

        Assert.Throws<DomainException>(() => fact.UpdateCosts(-1m, 100m, 50m));
    }
}
