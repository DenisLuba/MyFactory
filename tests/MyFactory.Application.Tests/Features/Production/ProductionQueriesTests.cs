using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Production.Commands.AssignWorker;
using MyFactory.Application.Features.Production.Commands.RecordProductionStage;
using MyFactory.Application.Features.Production.Queries.GetProductionOrderById;
using MyFactory.Application.Features.Production.Queries.GetProductionOrders;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Production;
using Xunit;

namespace MyFactory.Application.Tests.Features.Production;

public class ProductionQueriesTests
{
    [Fact]
    public async Task GetProductionOrderById_Should_Return_Stages_And_Assignments()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var (order, stageId, employee) = await SeedOrderWithAssignmentAsync(context);

        var handler = new GetProductionOrderByIdQueryHandler(context);
        var result = await handler.Handle(new GetProductionOrderByIdQuery(order.Id), default);

        var stage = result.Stages.Should().ContainSingle().Subject;
        stage.Assignments.Should().ContainSingle(assignment => assignment.EmployeeId == employee.Id);
    }

    [Fact]
    public async Task GetProductionOrders_Should_Filter_By_Status_And_Specification()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var completedSpec = ProductionTestHelper.CreateSpecification("SPEC-COMP");
        var inProgressSpec = ProductionTestHelper.CreateSpecification("SPEC-INPROG");
        var workshop = ProductionTestHelper.CreateWorkshop("Machining");

        context.Specifications.AddRange(completedSpec, inProgressSpec);
        context.Workshops.Add(workshop);

        var completedOrder = ProductionTestHelper.CreateProductionOrder(completedSpec, 15m);
        var inProgressOrder = ProductionTestHelper.CreateProductionOrder(inProgressSpec, 20m);

        context.ProductionOrders.AddRange(completedOrder, inProgressOrder);
        await context.SaveChangesAsync();

        var stageHandler = new RecordProductionStageCommandHandler(context);
        await stageHandler.Handle(new RecordProductionStageCommand(completedOrder.Id, workshop.Id, "Finish", 15m, 15m, DateTime.UtcNow), default);
        await stageHandler.Handle(new RecordProductionStageCommand(inProgressOrder.Id, workshop.Id, "Prep", 10m, 0m, DateTime.UtcNow), default);

        var orderToComplete = await context.ProductionOrders
            .Include(o => o.Stages)
            .FirstAsync(o => o.Id == completedOrder.Id);
        orderToComplete.Complete();
        await context.SaveChangesAsync();

        var handler = new GetProductionOrdersQueryHandler(context);
        var result = await handler.Handle(new GetProductionOrdersQuery(ProductionOrderStatuses.Completed, completedSpec.Id), default);

        result.Should().ContainSingle(order => order.Id == completedOrder.Id);
    }

    private static async Task<(ProductionOrder Order, Guid StageId, Domain.Entities.Employees.Employee Employee)> SeedOrderWithAssignmentAsync(TestApplicationDbContext context)
    {
        var specification = ProductionTestHelper.CreateSpecification("SPEC-QUERY");
        var workshop = ProductionTestHelper.CreateWorkshop("Assembly");
        var employee = ProductionTestHelper.CreateEmployee("Query Worker");
        var order = ProductionTestHelper.CreateProductionOrder(specification, 30m);

        context.Specifications.Add(specification);
        context.Workshops.Add(workshop);
        context.Employees.Add(employee);
        context.ProductionOrders.Add(order);
        await context.SaveChangesAsync();

        var stageHandler = new RecordProductionStageCommandHandler(context);
        await stageHandler.Handle(new RecordProductionStageCommand(order.Id, workshop.Id, "Welding", 12m, 0m, DateTime.UtcNow), default);

        var stage = await context.ProductionStages.SingleAsync();

        var assignHandler = new AssignWorkerCommandHandler(context);
        await assignHandler.Handle(new AssignWorkerCommand(stage.Id, employee.Id, 6m, 2m), default);

        var persistedStage = await context.ProductionStages
            .Include(s => s.Assignments)
            .SingleAsync();

        if (!persistedStage.Assignments.Any())
        {
            throw new InvalidOperationException("Stage assignments were not persisted.");
        }

        return (order, stage.Id, employee);
    }
}
