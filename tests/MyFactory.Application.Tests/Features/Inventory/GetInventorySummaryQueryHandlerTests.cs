using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Inventory.Queries.GetInventorySummary;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;
using Xunit;

namespace MyFactory.Application.Tests.Features.Inventory;

public sealed class GetInventorySummaryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAggregateInventoryByMaterial()
    {
        using var context = TestApplicationDbContextFactory.Create();

        var materialTypeId = Guid.NewGuid();
        var cotton = new Material("Cotton", materialTypeId, "kg");
        var nylon = new Material("Nylon", materialTypeId, "kg");
        var warehouseA = new Warehouse("Main", "Raw", "A1");
        var warehouseB = new Warehouse("Reserve", "Raw", "B1");

        var cottonItemA = warehouseA.AddInventory(cotton.Id);
        cottonItemA.Receive(100, 10m);
        cottonItemA.Reserve(20);

        var cottonItemB = warehouseB.AddInventory(cotton.Id);
        cottonItemB.Receive(50, 12m);

        var nylonItem = warehouseA.AddInventory(nylon.Id);
        nylonItem.Receive(80, 5m);

        await context.Materials.AddRangeAsync(cotton, nylon);
        await context.Warehouses.AddRangeAsync(warehouseA, warehouseB);
        await context.InventoryItems.AddRangeAsync(cottonItemA, cottonItemB, nylonItem);
        await context.SaveChangesAsync();

        var handler = new GetInventorySummaryQueryHandler(context);

        var result = await handler.Handle(new GetInventorySummaryQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);

        var cottonSummary = result.Single(summary => summary.MaterialId == cotton.Id);
        Assert.Equal(150m, cottonSummary.TotalQuantity);
        Assert.Equal(20m, cottonSummary.ReservedQuantity);
        Assert.Equal(130m, cottonSummary.AvailableQuantity);
        Assert.Equal((100m * 10m) + (50m * 12m), cottonSummary.TotalValue);
        Assert.Equal(2, cottonSummary.Warehouses.Count);

        var nylonSummary = result.Single(summary => summary.MaterialId == nylon.Id);
        Assert.Equal(80m, nylonSummary.TotalQuantity);
        Assert.Single(nylonSummary.Warehouses);
    }
}
