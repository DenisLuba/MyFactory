using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Warehousing.Commands.ReserveInventory;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class ReserveInventoryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReserveQuantity()
    {
        var setup = await SeedInventoryAsync(quantity: 20m, unitPrice: 5m);
        await using var context = setup.Context;
        var handler = new ReserveInventoryCommandHandler(context);

        var result = await handler.Handle(new ReserveInventoryCommand(setup.Warehouse.Id, setup.Material.Id, 5m), CancellationToken.None);

        result.ReservedQuantity.Should().Be(5m);
        var stored = await context.InventoryItems.AsNoTracking().SingleAsync();
        stored.ReservedQuantity.Should().Be(5m);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenReservingMoreThanAvailable()
    {
        var setup = await SeedInventoryAsync(quantity: 5m, unitPrice: 5m);
        await using var context = setup.Context;
        var handler = new ReserveInventoryCommandHandler(context);

        var act = async () => await handler.Handle(new ReserveInventoryCommand(setup.Warehouse.Id, setup.Material.Id, 10m), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }

    private static async Task<(TestApplicationDbContext Context, Warehouse Warehouse, Material Material)> SeedInventoryAsync(decimal quantity, decimal unitPrice)
    {
        var context = TestApplicationDbContextFactory.Create();
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var type = new MaterialType("Metals");
        var material = new Material("Steel", type.Id, "kg");
        var inventoryItem = new InventoryItem(warehouse.Id, material.Id);
        inventoryItem.Receive(quantity, unitPrice);

        await context.Warehouses.AddAsync(warehouse);
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.InventoryItems.AddAsync(inventoryItem);
        await context.SaveChangesAsync();

        return (context, warehouse, material);
    }
}
