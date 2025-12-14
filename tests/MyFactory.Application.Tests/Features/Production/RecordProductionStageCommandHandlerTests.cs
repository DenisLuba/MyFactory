using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Production.Commands.RecordProductionStage;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Application.Tests.Features.Production;

public class RecordProductionStageCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Record_Stage_And_Start_Order()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (order, workshop) = await SeedOrderAsync(context, 80m);
        var handler = new RecordProductionStageCommandHandler(context);
        var recordedAt = DateTime.UtcNow;

        var result = await handler.Handle(new RecordProductionStageCommand(order.Id, workshop.Id, "Cutting", 20m, 15m, recordedAt), default);

        result.Status.Should().Be(ProductionOrderStatuses.InProgress);
        result.Stages.Should().ContainSingle(stage => stage.StageType == "Cutting" && stage.QuantityIn == 20m && stage.QuantityOut == 15m);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Output_Exceeds_Input()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (order, workshop) = await SeedOrderAsync(context, 30m);
        var handler = new RecordProductionStageCommandHandler(context);

        var act = async () => await handler.Handle(new RecordProductionStageCommand(order.Id, workshop.Id, "Assembly", 10m, 20m, DateTime.UtcNow), default);

        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Order_Not_Found()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var handler = new RecordProductionStageCommandHandler(context);

        var act = async () => await handler.Handle(new RecordProductionStageCommand(Guid.NewGuid(), Guid.NewGuid(), "Painting", 5m, 0m, DateTime.UtcNow), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static async Task<(ProductionOrder Order, Domain.Entities.Workshops.Workshop Workshop)> SeedOrderAsync(
        TestApplicationDbContext context,
        decimal quantity)
    {
        var specification = ProductionTestHelper.CreateSpecification("SPEC-STAGE");
        var workshop = ProductionTestHelper.CreateWorkshop("Fabrication");
        var order = ProductionTestHelper.CreateProductionOrder(specification, quantity);

        context.Specifications.Add(specification);
        context.Workshops.Add(workshop);
        context.ProductionOrders.Add(order);
        await context.SaveChangesAsync();

        return (order, workshop);
    }
}
