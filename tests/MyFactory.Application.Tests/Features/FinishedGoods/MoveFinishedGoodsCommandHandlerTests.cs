using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.FinishedGoods.Commands.MoveFinishedGoods;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

public class MoveFinishedGoodsCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Move_Inventory_Between_Warehouses()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-MOVE-1");
        var fromWarehouse = FinishedGoodsTestHelper.CreateWarehouse("FG-FROM");
        var toWarehouse = FinishedGoodsTestHelper.CreateWarehouse("FG-TO");
        var fromInventory = FinishedGoodsTestHelper.CreateInventory(specification, fromWarehouse, 10m, 25m);

        context.Specifications.Add(specification);
        context.Warehouses.AddRange(fromWarehouse, toWarehouse);
        context.FinishedGoodsInventories.Add(fromInventory);
        await context.SaveChangesAsync();

        var handler = new MoveFinishedGoodsCommandHandler(context);
        var movedAt = DateTime.UtcNow;

        var result = await handler.Handle(
            new MoveFinishedGoodsCommand(specification.Id, fromWarehouse.Id, toWarehouse.Id, 4m, movedAt),
            default);

        result.Quantity.Should().Be(4m);

        var remaining = await context.FinishedGoodsInventories.SingleAsync(entity => entity.WarehouseId == fromWarehouse.Id);
        remaining.Quantity.Should().Be(6m);

        var destination = await context.FinishedGoodsInventories.SingleAsync(entity => entity.WarehouseId == toWarehouse.Id);
        destination.Quantity.Should().Be(4m);
        destination.UnitCost.Should().Be(25m);

        var movement = await context.FinishedGoodsMovements.SingleAsync();
        movement.Quantity.Should().Be(4m);
        movement.MovedAt.Should().Be(movedAt);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Insufficient_Inventory()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-MOVE-FAIL");
        var fromWarehouse = FinishedGoodsTestHelper.CreateWarehouse("FG-FROM-FAIL");
        var toWarehouse = FinishedGoodsTestHelper.CreateWarehouse("FG-TO-FAIL");
        var fromInventory = FinishedGoodsTestHelper.CreateInventory(specification, fromWarehouse, 2m, 30m);

        context.Specifications.Add(specification);
        context.Warehouses.AddRange(fromWarehouse, toWarehouse);
        context.FinishedGoodsInventories.Add(fromInventory);
        await context.SaveChangesAsync();

        var handler = new MoveFinishedGoodsCommandHandler(context);

        var act = async () => await handler.Handle(
            new MoveFinishedGoodsCommand(specification.Id, fromWarehouse.Id, toWarehouse.Id, 5m, DateTime.UtcNow),
            default);

        await act.Should().ThrowAsync<DomainException>();
    }
}
