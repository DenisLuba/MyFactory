using System;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Shifts.Commands.UpdateShiftPlan;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Shifts;
using Xunit;

namespace MyFactory.Application.Tests.Features.Shifts;

public class UpdateShiftPlanCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_ShiftType_And_PlannedQty()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Update Employee");
        var specification = ShiftsTestHelper.CreateSpecification("UPDATE-SPEC");
        var plan = new ShiftPlan(employee.Id, specification.Id, new DateOnly(2025, 4, 1), "Day", 90);

        context.Employees.Add(employee);
        context.Specifications.Add(specification);
        context.ShiftPlans.Add(plan);
        await context.SaveChangesAsync();

        var handler = new UpdateShiftPlanCommandHandler(context);

        var result = await handler.Handle(new UpdateShiftPlanCommand(plan.Id, "Night", 140), default);

        result.ShiftType.Should().Be("Night");
        result.PlannedQuantity.Should().Be(140);
    }

    [Fact]
    public async Task Handle_Should_Not_Modify_Immutable_Fields()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Immutable Employee");
        var specification = ShiftsTestHelper.CreateSpecification("IMMUTABLE-SPEC");
        var shiftDate = new DateOnly(2025, 4, 2);
        var plan = new ShiftPlan(employee.Id, specification.Id, shiftDate, "Day", 110);

        context.Employees.Add(employee);
        context.Specifications.Add(specification);
        context.ShiftPlans.Add(plan);
        await context.SaveChangesAsync();

        var handler = new UpdateShiftPlanCommandHandler(context);

        var result = await handler.Handle(new UpdateShiftPlanCommand(plan.Id, "Swing", 115), default);

        result.EmployeeId.Should().Be(employee.Id);
        result.SpecificationId.Should().Be(specification.Id);
        result.ShiftDate.Should().Be(shiftDate);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Plan_Not_Found()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var handler = new UpdateShiftPlanCommandHandler(context);

        var act = async () => await handler.Handle(new UpdateShiftPlanCommand(Guid.NewGuid(), "Day", 50), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Invalid_Quantity()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Bad Qty Employee");
        var specification = ShiftsTestHelper.CreateSpecification("BAD-QTY");
        var plan = new ShiftPlan(employee.Id, specification.Id, new DateOnly(2025, 4, 3), "Day", 80);

        context.Employees.Add(employee);
        context.Specifications.Add(specification);
        context.ShiftPlans.Add(plan);
        await context.SaveChangesAsync();

        var handler = new UpdateShiftPlanCommandHandler(context);

        var act = async () => await handler.Handle(new UpdateShiftPlanCommand(plan.Id, "Night", 0), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
