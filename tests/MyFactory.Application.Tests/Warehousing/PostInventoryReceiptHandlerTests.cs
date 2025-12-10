using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Warehousing.Commands.PostInventoryReceipt;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;
using Xunit;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class PostInventoryReceiptHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateInventoryAndMarkReceiptReceived()
    {
        var setup = await SeedReceiptWithItemsAsync();
        await using var context = setup.Context;
        var handler = new PostInventoryReceiptCommandHandler(context);

        var result = await handler.Handle(new PostInventoryReceiptCommand(setup.Receipt.Id, setup.Warehouse.Id), CancellationToken.None);

        result.Status.Should().Be(InventoryReceiptStatuses.Received);
        var inventoryItem = await context.InventoryItems.AsNoTracking().SingleAsync();
        inventoryItem.Quantity.Should().Be(20m);
        inventoryItem.AveragePrice.Should().Be(5m);
    }

    [Fact]
    public async Task Handle_ShouldNotAllowPostingTwice()
    {
        var setup = await SeedReceiptWithItemsAsync();
        await using var context = setup.Context;
        var handler = new PostInventoryReceiptCommandHandler(context);
        await handler.Handle(new PostInventoryReceiptCommand(setup.Receipt.Id, setup.Warehouse.Id), CancellationToken.None);

        var act = async () => await handler.Handle(new PostInventoryReceiptCommand(setup.Receipt.Id, setup.Warehouse.Id), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenReceiptHasNoItems()
    {
        var setup = await SeedEmptyReceiptAsync();
        await using var context = setup.Context;
        var handler = new PostInventoryReceiptCommandHandler(context);

        var act = async () => await handler.Handle(new PostInventoryReceiptCommand(setup.Receipt.Id, setup.Warehouse.Id), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static async Task<(TestApplicationDbContext Context, Warehouse Warehouse, InventoryReceipt Receipt)> SeedReceiptWithItemsAsync()
    {
        var context = TestApplicationDbContextFactory.Create();
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var type = new MaterialType("Metals");
        var material = new Material("Steel", type.Id, "kg");
        var supplier = new Supplier("Acme", "contact");
        var receipt = new InventoryReceipt("RC-POST", supplier.Id, DateTime.UtcNow);
        receipt.AddItem(material.Id, 10m, 6m);

        var inventoryItem = new InventoryItem(warehouse.Id, material.Id);
        inventoryItem.Receive(10m, 4m);

        await context.Warehouses.AddAsync(warehouse);
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.Suppliers.AddAsync(supplier);
        await context.InventoryReceipts.AddAsync(receipt);
        await context.InventoryItems.AddAsync(inventoryItem);
        await context.SaveChangesAsync();

        return (context, warehouse, receipt);
    }

    private static async Task<(TestApplicationDbContext Context, Warehouse Warehouse, InventoryReceipt Receipt)> SeedEmptyReceiptAsync()
    {
        var context = TestApplicationDbContextFactory.Create();
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var supplier = new Supplier("Acme", "contact");
        var receipt = new InventoryReceipt("RC-EMPTY", supplier.Id, DateTime.UtcNow);

        await context.Warehouses.AddAsync(warehouse);
        await context.Suppliers.AddAsync(supplier);
        await context.InventoryReceipts.AddAsync(receipt);
        await context.SaveChangesAsync();

        return (context, warehouse, receipt);
    }
}
