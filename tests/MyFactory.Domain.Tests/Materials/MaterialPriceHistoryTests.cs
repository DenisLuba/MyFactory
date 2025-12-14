using System;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Materials;

public class MaterialPriceHistoryTests
{
    [Fact]
    public void SetEffectiveTo_WithValidDate_AssignsValue()
    {
        var effectiveFrom = new DateOnly(2025, 1, 1);
        var entry = new MaterialPriceHistory(Guid.NewGuid(), Guid.NewGuid(), 9.99m, effectiveFrom, "DOC-123");
        var effectiveTo = effectiveFrom.AddDays(10);

        entry.SetEffectiveTo(effectiveTo);

        Assert.Equal(effectiveTo, entry.EffectiveTo);
        Assert.Equal("DOC-123", entry.DocRef.Value);
    }

    [Fact]
    public void SetEffectiveTo_BeforeEffectiveFrom_Throws()
    {
        var effectiveFrom = new DateOnly(2025, 1, 1);
        var entry = new MaterialPriceHistory(Guid.NewGuid(), Guid.NewGuid(), 9.99m, effectiveFrom, "DOC-123");

        Assert.Throws<DomainException>(() => entry.SetEffectiveTo(effectiveFrom.AddDays(-1)));
    }
}
