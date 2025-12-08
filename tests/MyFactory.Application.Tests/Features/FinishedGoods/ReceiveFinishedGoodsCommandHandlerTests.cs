using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.FinishedGoods.Commands.ReceiveFinishedGoods;
using MyFactory.Application.Tests.Common;
using Xunit;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

public class ReceiveFinishedGoodsCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_New_Inventory()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-REC-NEW");
        var warehouse = FinishedGoodsTestHelper.CreateWarehouse("FG-REC-1");
        context.Specifications.Add(specification);
        context.Warehouses.Add(warehouse);
        await context.SaveChangesAsync();

        var handler = new ReceiveFinishedGoodsCommandHandler(context);
        var receivedAt = DateTime.UtcNow;

        var result = await handler.Handle(
            new ReceiveFinishedGoodsCommand(specification.Id, warehouse.Id, 10m, 50m, receivedAt),
            default);

        result.Quantity.Should().Be(10m);
        result.UnitCost.Should().Be(50m);

        var inventory = await context.FinishedGoodsInventories.SingleAsync();
        inventory.Quantity.Should().Be(10m);
        inventory.UnitCost.Should().Be(50m);
    }

    [Fact]
    public async Task Handle_Should_Update_Existing_Inventory()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-REC-EXISTING");
        var warehouse = FinishedGoodsTestHelper.CreateWarehouse("FG-REC-2");
        var inventory = FinishedGoodsTestHelper.CreateInventory(specification, warehouse, 5m, 40m);

        context.Specifications.Add(specification);
        context.Warehouses.Add(warehouse);
        context.FinishedGoodsInventories.Add(inventory);
        await context.SaveChangesAsync();

        var handler = new ReceiveFinishedGoodsCommandHandler(context);

        var result = await handler.Handle(
            new ReceiveFinishedGoodsCommand(specification.Id, warehouse.Id, 5m, 60m, DateTime.UtcNow),
            default);

        result.Quantity.Should().Be(10m);
        result.UnitCost.Should().BeApproximately(50m, 0.0001m);

        var updated = await context.FinishedGoodsInventories.SingleAsync();
        updated.Quantity.Should().Be(10m);
        updated.UnitCost.Should().BeApproximately(50m, 0.0001m);
    }
}
