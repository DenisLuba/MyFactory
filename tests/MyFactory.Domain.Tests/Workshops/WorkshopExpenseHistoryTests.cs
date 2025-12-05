using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Workshops;
using Xunit;

namespace MyFactory.Domain.Tests.Workshops;

public class WorkshopExpenseHistoryTests
{
    [Fact]
    public void ClosePeriod_WithValidDate_SetsEffectiveTo()
    {
        var entry = new WorkshopExpenseHistory(Guid.NewGuid(), Guid.NewGuid(), 3.25m, new DateTime(2025, 2, 1));
        var effectiveTo = new DateTime(2025, 2, 28);

        entry.ClosePeriod(effectiveTo);

        Assert.Equal(effectiveTo, entry.EffectiveTo);
    }

    [Fact]
    public void ClosePeriod_WithEarlierDate_Throws()
    {
        var entry = new WorkshopExpenseHistory(Guid.NewGuid(), Guid.NewGuid(), 3.25m, new DateTime(2025, 2, 10));

        Assert.Throws<DomainException>(() => entry.ClosePeriod(new DateTime(2025, 2, 5)));
    }
}
