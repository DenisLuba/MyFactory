using System;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Production.Commands.CreateProductionOrder;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Production;
using Xunit;

namespace MyFactory.Application.Tests.Features.Production;

public class CreateProductionOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Order()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = ProductionTestHelper.CreateSpecification("SPEC-001");
        context.Specifications.Add(specification);
        await context.SaveChangesAsync();

        var handler = new CreateProductionOrderCommandHandler(context);

        var result = await handler.Handle(new CreateProductionOrderCommand("PO-100", specification.Id, 25m), default);

        result.OrderNumber.Should().Be("PO-100");
        result.Status.Should().Be(ProductionOrderStatus.Planned);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Specification_Missing()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var handler = new CreateProductionOrderCommandHandler(context);

        var act = async () => await handler.Handle(new CreateProductionOrderCommand("PO-101", Guid.NewGuid(), 10m), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_OrderNumber_Not_Unique()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = ProductionTestHelper.CreateSpecification("SPEC-002");
        var existingOrder = ProductionTestHelper.CreateProductionOrder(specification, 50m);
        context.Specifications.Add(specification);
        context.ProductionOrders.Add(existingOrder);
        await context.SaveChangesAsync();

        var handler = new CreateProductionOrderCommandHandler(context);

        var act = async () => await handler.Handle(new CreateProductionOrderCommand(existingOrder.OrderNumber, specification.Id, 10m), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Quantity_Not_Positive()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = ProductionTestHelper.CreateSpecification("SPEC-003");
        context.Specifications.Add(specification);
        await context.SaveChangesAsync();

        var handler = new CreateProductionOrderCommandHandler(context);

        var act = async () => await handler.Handle(new CreateProductionOrderCommand("PO-102", specification.Id, 0m), default);

        await act.Should().ThrowAsync<DomainException>();
    }
}
