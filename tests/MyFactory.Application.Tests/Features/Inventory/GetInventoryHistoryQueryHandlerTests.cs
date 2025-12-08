using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Inventory.Queries.GetInventoryHistory;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;
using Xunit;

namespace MyFactory.Application.Tests.Features.Inventory;

public sealed class GetInventoryHistoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldFilterHistoryByWarehouseAndDate()
    {
        using var context = TestApplicationDbContextFactory.Create();

        var now = DateTime.UtcNow;
        var material = new Material("Cotton", Guid.NewGuid(), "kg");
        var supplier = new Supplier("Supplier", "contact");
        var warehouseA = new Warehouse("Main", "Raw", "A1");
        var warehouseB = new Warehouse("Reserve", "Raw", "B1");
        var inventoryA = warehouseA.AddInventory(material.Id);
        inventoryA.Receive(100, 10m);
        var inventoryB = warehouseB.AddInventory(material.Id);
        inventoryB.Receive(50, 11m);

        var receiptOlder = new InventoryReceipt("RCPT-1", supplier.Id, now.AddDays(-5));
        receiptOlder.AddItem(material.Id, 40, 9m, inventoryA.Id);
        receiptOlder.MarkAsReceived();

        var receiptNewer = new InventoryReceipt("RCPT-2", supplier.Id, now.AddDays(-1));
        receiptNewer.AddItem(material.Id, 30, 12m, inventoryB.Id);
        receiptNewer.MarkAsReceived();

        await context.Materials.AddAsync(material);
        await context.Suppliers.AddAsync(supplier);
        await context.Warehouses.AddRangeAsync(warehouseA, warehouseB);
        await context.InventoryItems.AddRangeAsync(inventoryA, inventoryB);
        await context.InventoryReceipts.AddRangeAsync(receiptOlder, receiptNewer);
        await context.SaveChangesAsync();

        var handler = new GetInventoryHistoryQueryHandler(context);
        var query = new GetInventoryHistoryQuery(material.Id, warehouseB.Id, now.AddDays(-2), now);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Single(result);
        var entry = result.Single();
        Assert.Equal("RCPT-2", entry.ReceiptNumber);
        Assert.Equal(warehouseB.Id, entry.WarehouseId);
        Assert.Equal(30m, entry.Quantity);
        Assert.Equal(30m * 12m, entry.LineTotal);
    }
}
