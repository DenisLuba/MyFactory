using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata;
using MyFactory.Domain.Entities.FinishedGoods;
using MyFactory.Domain.Entities.Materials;
using Xunit;

namespace MyFactory.Infrastructure.Tests;

public class ModelConfigurationTests
{
    [Fact]
    public void FinishedGoodsInventory_ShouldHaveUniqueSpecificationWarehouseIndex()
    {
        using var context = TestDbContextFactory.CreateContext();
        var entityType = context.Model.FindEntityType(typeof(FinishedGoodsInventory));
        entityType.Should().NotBeNull();

        var index = entityType!
            .GetIndexes()
            .FirstOrDefault(idx => idx.IsUnique && idx.Properties.Select(p => p.Name).SequenceEqual(new[] { "SpecificationId", "WarehouseId" }));

        index.Should().NotBeNull();
    }

    [Fact]
    public void MaterialName_ShouldRespectConfiguredLength()
    {
        using var context = TestDbContextFactory.CreateContext();
        var entityType = context.Model.FindEntityType(typeof(Material));
        var property = entityType!.FindProperty(nameof(Material.Name));
        property.Should().NotBeNull();
        property!.GetMaxLength().Should().Be(256);
    }
}
