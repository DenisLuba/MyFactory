using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Inventory.Queries.GetMaterialInventory;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;
using Xunit;

namespace MyFactory.Application.Tests.Features.Inventory;

public sealed class GetMaterialInventoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMaterialSummary()
    {
        using var context = TestApplicationDbContextFactory.Create();

        var materialTypeId = Guid.NewGuid();
        var cotton = new Material("Cotton", materialTypeId, "kg");
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var item = warehouse.AddInventory(cotton.Id);
        item.Receive(75, 8m);
        item.Reserve(5);

        await context.Materials.AddAsync(cotton);
        await context.Warehouses.AddAsync(warehouse);
        await context.InventoryItems.AddAsync(item);
        await context.SaveChangesAsync();

        var handler = new GetMaterialInventoryQueryHandler(context);

        var result = await handler.Handle(new GetMaterialInventoryQuery(cotton.Id), CancellationToken.None);

        Assert.Equal(75m, result.TotalQuantity);
        Assert.Equal(5m, result.ReservedQuantity);
        Assert.Equal(70m, result.AvailableQuantity);
        Assert.Single(result.Warehouses);
    }

    [Fact]
    public async Task Handle_ShouldReturnZerosWhenNoInventory()
    {
        using var context = TestApplicationDbContextFactory.Create();

        var material = new Material("Nylon", Guid.NewGuid(), "kg");
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();

        var handler = new GetMaterialInventoryQueryHandler(context);

        var result = await handler.Handle(new GetMaterialInventoryQuery(material.Id), CancellationToken.None);

        Assert.Equal(0m, result.TotalQuantity);
        Assert.Empty(result.Warehouses);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMaterialMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetMaterialInventoryQueryHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetMaterialInventoryQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
