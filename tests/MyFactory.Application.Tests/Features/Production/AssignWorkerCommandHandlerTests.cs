using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Production.Commands.AssignWorker;
using MyFactory.Application.Features.Production.Commands.RecordProductionStage;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Application.Tests.Features.Production;

public class AssignWorkerCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Assignment_And_Complete_Work()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (order, stageId) = await SeedStageAsync(context);
        var employee = ProductionTestHelper.CreateEmployee("Alice Operator");
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var handler = new AssignWorkerCommandHandler(context);

        var result = await handler.Handle(new AssignWorkerCommand(stageId, employee.Id, 5m, 5m), default);

        var stage = result.Stages.Single();
        stage.Assignments.Should().ContainSingle(assignment => assignment.EmployeeId == employee.Id && assignment.QuantityCompleted == 5m);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Employee_Not_Found()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (_, stageId) = await SeedStageAsync(context);
        var handler = new AssignWorkerCommandHandler(context);

        var act = async () => await handler.Handle(new AssignWorkerCommand(stageId, Guid.NewGuid(), 3m, 0m), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Completed_Exceeds_Assigned()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (_, stageId) = await SeedStageAsync(context);
        var employee = ProductionTestHelper.CreateEmployee("Bob Worker");
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var handler = new AssignWorkerCommandHandler(context);

        var act = async () => await handler.Handle(new AssignWorkerCommand(stageId, employee.Id, 4m, 6m), default);

        await act.Should().ThrowAsync<DomainException>();
    }

    private static async Task<(Domain.Entities.Production.ProductionOrder Order, Guid StageId)> SeedStageAsync(TestApplicationDbContext context)
    {
        var specification = ProductionTestHelper.CreateSpecification("SPEC-WORKER");
        var workshop = ProductionTestHelper.CreateWorkshop("Finishing");
        var order = ProductionTestHelper.CreateProductionOrder(specification, 40m);

        context.Specifications.Add(specification);
        context.Workshops.Add(workshop);
        context.ProductionOrders.Add(order);
        await context.SaveChangesAsync();

        var stageHandler = new RecordProductionStageCommandHandler(context);
        await stageHandler.Handle(new RecordProductionStageCommand(order.Id, workshop.Id, "Prep", 10m, 0m, DateTime.UtcNow), default);

        var stage = await context.ProductionStages.SingleAsync();
        return (order, stage.Id);
    }
}
