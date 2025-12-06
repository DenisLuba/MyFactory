using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Warehousing.Commands.AdjustInventory;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class AdjustInventoryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldIncreaseQuantityWithAveragePrice()
    {
        var setup = await SeedWarehouseAndMaterialAsync();
        await using var context = setup.Context;
        var handler = new AdjustInventoryCommandHandler(context);
        var command = new AdjustInventoryCommand(setup.Warehouse.Id, setup.Material.Id, 10m, 5m);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Quantity.Should().Be(10m);
        result.AveragePrice.Should().Be(5m);
        var stored = await context.InventoryItems.AsNoTracking().SingleAsync();
        stored.Quantity.Should().Be(10m);
    }

    [Fact]
    public async Task Handle_ShouldDecreaseQuantity()
    {
        var setup = await SeedWarehouseAndMaterialAsync();
        await using var context = setup.Context;
        var handler = new AdjustInventoryCommandHandler(context);
        await handler.Handle(new AdjustInventoryCommand(setup.Warehouse.Id, setup.Material.Id, 10m, 5m), CancellationToken.None);

        var result = await handler.Handle(new AdjustInventoryCommand(setup.Warehouse.Id, setup.Material.Id, -3m, null), CancellationToken.None);

        result.Quantity.Should().Be(7m);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenDecreaseBelowZero()
    {
        var setup = await SeedWarehouseAndMaterialAsync();
        await using var context = setup.Context;
        var handler = new AdjustInventoryCommandHandler(context);

        var act = async () => await handler.Handle(new AdjustInventoryCommand(setup.Warehouse.Id, setup.Material.Id, -1m, null), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_ShouldRecalculateAveragePrice()
    {
        var setup = await SeedWarehouseAndMaterialAsync();
        await using var context = setup.Context;
        var handler = new AdjustInventoryCommandHandler(context);

        await handler.Handle(new AdjustInventoryCommand(setup.Warehouse.Id, setup.Material.Id, 10m, 5m), CancellationToken.None);
        var result = await handler.Handle(new AdjustInventoryCommand(setup.Warehouse.Id, setup.Material.Id, 5m, 10m), CancellationToken.None);

        result.Quantity.Should().Be(15m);
        result.AveragePrice.Should().Be((10m * 5m + 5m * 10m) / 15m);
    }

    private static async Task<(TestApplicationDbContext Context, Warehouse Warehouse, Material Material)> SeedWarehouseAndMaterialAsync()
    {
        var context = TestApplicationDbContextFactory.Create();
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var type = new MaterialType("Metals");
        var material = new Material("Steel", type.Id, "kg");
        await context.Warehouses.AddAsync(warehouse);
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();
        return (context, warehouse, material);
    }
}
