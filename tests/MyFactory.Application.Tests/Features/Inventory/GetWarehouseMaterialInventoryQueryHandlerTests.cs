using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Inventory.Queries.GetWarehouseMaterialInventory;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;
using Xunit;

namespace MyFactory.Application.Tests.Features.Inventory;

public sealed class GetWarehouseMaterialInventoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnWarehouseMaterialInventory()
    {
        using var context = TestApplicationDbContextFactory.Create();

        var material = new Material("Cotton", Guid.NewGuid(), "kg");
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var item = warehouse.AddInventory(material.Id);
        item.Receive(40, 9m);

        await context.Materials.AddAsync(material);
        await context.Warehouses.AddAsync(warehouse);
        await context.InventoryItems.AddAsync(item);
        await context.SaveChangesAsync();

        var handler = new GetWarehouseMaterialInventoryQueryHandler(context);

        var result = await handler.Handle(new GetWarehouseMaterialInventoryQuery(warehouse.Id, material.Id), CancellationToken.None);

        Assert.Equal(material.Id, result.MaterialId);
        Assert.Equal(40m, result.Quantity);
        Assert.Equal(warehouse.Id, result.WarehouseId);
        Assert.Equal(40m * 9m, result.TotalValue);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenInventoryMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetWarehouseMaterialInventoryQueryHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetWarehouseMaterialInventoryQuery(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None));
    }
}
