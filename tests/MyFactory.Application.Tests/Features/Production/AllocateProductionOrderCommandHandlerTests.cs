using System;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Production.Commands.AllocateProductionOrder;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Application.Tests.Features.Production;

public class AllocateProductionOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Add_Allocation()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (order, workshop) = await SeedOrderAsync(context, quantity: 100m);
        var handler = new AllocateProductionOrderCommandHandler(context);

        var result = await handler.Handle(new AllocateProductionOrderCommand(order.Id, workshop.Id, 40m), default);

        result.Allocations.Should().ContainSingle(allocation => allocation.WorkshopId == workshop.Id && allocation.QuantityAllocated == 40m);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_OverAllocated()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (order, workshop) = await SeedOrderAsync(context, quantity: 50m);
        var handler = new AllocateProductionOrderCommandHandler(context);

        await handler.Handle(new AllocateProductionOrderCommand(order.Id, workshop.Id, 40m), default);

        var act = async () => await handler.Handle(new AllocateProductionOrderCommand(order.Id, workshop.Id, 20m), default);

        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Workshop_Missing()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (order, _) = await SeedOrderAsync(context, quantity: 20m, persistWorkshop: false);
        var handler = new AllocateProductionOrderCommandHandler(context);

        var act = async () => await handler.Handle(new AllocateProductionOrderCommand(order.Id, Guid.NewGuid(), 10m), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static async Task<(Domain.Entities.Production.ProductionOrder Order, Domain.Entities.Workshops.Workshop Workshop)> SeedOrderAsync(
        TestApplicationDbContext context,
        decimal quantity,
        bool persistWorkshop = true)
    {
        var specification = ProductionTestHelper.CreateSpecification("SPEC-ALLOC");
        var workshop = ProductionTestHelper.CreateWorkshop("Assembly");
        var order = ProductionTestHelper.CreateProductionOrder(specification, quantity);

        context.Specifications.Add(specification);
        if (persistWorkshop)
        {
            context.Workshops.Add(workshop);
        }
        context.ProductionOrders.Add(order);
        await context.SaveChangesAsync();

        return (order, workshop);
    }
}
