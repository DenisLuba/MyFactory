using System;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Domain.Entities.FinishedGoods;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;
using Xunit;

namespace MyFactory.Infrastructure.Tests;

public class AuditBehaviorTests
{
    [Fact]
    public async Task SpecificationCreatedAt_ShouldRemainDomainValue()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var createdAt = DateTime.UtcNow.AddMonths(-2);
        var specification = new Specification("SKU-500", "Legacy Assembly", 6.5m, "Active", createdAt);

        await context.Specifications.AddAsync(specification);
        await context.SaveChangesAsync();

        specification.CreatedAt.Should().Be(createdAt);
        specification.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public async Task FinishedGoodsInventory_ShouldKeepManualUpdatedAt()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var specification = new Specification("SKU-900", "Finished Widget", 15m, "Active", DateTime.UtcNow.AddDays(-5));
        var warehouse = new Warehouse("FG Warehouse", "Finished", "Zone 3");
        await context.Specifications.AddAsync(specification);
        await context.Warehouses.AddAsync(warehouse);
        await context.SaveChangesAsync();

        var inventory = new FinishedGoodsInventory(specification.Id, warehouse.Id);
        var receivedAt = DateTime.UtcNow.AddHours(-4);
        inventory.Receive(25, 8.5m, receivedAt);
        await context.FinishedGoodsInventories.AddAsync(inventory);
        await context.SaveChangesAsync();

        inventory.UpdatedAt.Should().Be(receivedAt);
    }

    [Fact]
    public async Task MaterialUpdate_ShouldStampUpdatedAt()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var materialType = new MaterialType("Metals");
        await context.MaterialTypes.AddAsync(materialType);
        var material = new Material("Steel Rod", materialType.Id, "kg");
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();

        var originalCreated = material.CreatedAt;
        material.UpdateName("Steel Rod 18mm");
        await context.SaveChangesAsync();

        material.UpdatedAt.Should().NotBeNull();
        material.UpdatedAt!.Value.Should().BeAfter(originalCreated);
    }
}
