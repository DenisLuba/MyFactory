using System;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using Xunit;

namespace MyFactory.Domain.Tests.Materials;

public class MaterialTests
{
    [Fact]
    public void AddPrice_WithValidData_AddsHistoryEntry()
    {
        var materialTypeId = Guid.NewGuid();
        var material = new Material("Cotton Fabric", materialTypeId, "kg");
        var supplierId = Guid.NewGuid();
        var effectiveFrom = new DateTime(2025, 1, 1);

        var entry = material.AddPrice(supplierId, 12.5m, effectiveFrom, "DOC-001");

        Assert.Single(material.PriceHistory);
        Assert.Equal(entry, material.PriceHistory.Single());
        Assert.Equal(supplierId, entry.SupplierId);
        Assert.Equal(12.5m, entry.Price);
        Assert.Equal(effectiveFrom, entry.EffectiveFrom);
    }

    [Fact]
    public void AddPrice_WithNonPositivePrice_Throws()
    {
        var material = new Material("Buttons", Guid.NewGuid(), "pcs");

        Assert.Throws<DomainException>(() =>
            material.AddPrice(Guid.NewGuid(), 0m, DateTime.UtcNow, "DOC-ERROR"));
    }
}
