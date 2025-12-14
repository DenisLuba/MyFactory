using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Warehousing.Commands.ReleaseInventoryReservation;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class ReleaseInventoryReservationHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReleaseReservation()
    {
        var setup = await SeedReservedInventoryAsync(reserved: 5m);
        await using var context = setup.Context;
        var handler = new ReleaseInventoryReservationCommandHandler(context);

        var result = await handler.Handle(new ReleaseInventoryReservationCommand(setup.Warehouse.Id, setup.Material.Id, 3m), CancellationToken.None);

        result.ReservedQuantity.Should().Be(2m);
        var stored = await context.InventoryItems.AsNoTracking().SingleAsync();
        stored.ReservedQuantity.Should().Be(2m);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenReleaseExceedsReserved()
    {
        var setup = await SeedReservedInventoryAsync(reserved: 2m);
        await using var context = setup.Context;
        var handler = new ReleaseInventoryReservationCommandHandler(context);

        var act = async () => await handler.Handle(new ReleaseInventoryReservationCommand(setup.Warehouse.Id, setup.Material.Id, 3m), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }

    private static async Task<(TestApplicationDbContext Context, Warehouse Warehouse, Material Material)> SeedReservedInventoryAsync(decimal reserved)
    {
        var context = TestApplicationDbContextFactory.Create();
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var type = new MaterialType("Metals");
        var material = new Material("Steel", type.Id, "kg");
        var inventoryItem = new InventoryItem(warehouse.Id, material.Id);
        inventoryItem.Receive(10m, 5m);
        inventoryItem.Reserve(reserved);

        await context.Warehouses.AddAsync(warehouse);
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.InventoryItems.AddAsync(inventoryItem);
        await context.SaveChangesAsync();

        return (context, warehouse, material);
    }
}
