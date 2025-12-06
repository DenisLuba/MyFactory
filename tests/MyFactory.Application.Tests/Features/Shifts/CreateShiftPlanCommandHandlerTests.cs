using System;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Shifts.Commands.CreateShiftPlan;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Shifts;
using Xunit;

namespace MyFactory.Application.Tests.Features.Shifts;

public class CreateShiftPlanCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_ShiftPlan()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Create Plan Employee");
        var specification = ShiftsTestHelper.CreateSpecification("SHIFT-SPEC");

        context.Employees.Add(employee);
        context.Specifications.Add(specification);
        await context.SaveChangesAsync();

        var handler = new CreateShiftPlanCommandHandler(context);
        var shiftDate = new DateOnly(2025, 3, 5);

        var result = await handler.Handle(new CreateShiftPlanCommand(employee.Id, specification.Id, shiftDate, "Day", 120), default);

        result.EmployeeId.Should().Be(employee.Id);
        result.SpecificationId.Should().Be(specification.Id);
        result.ShiftDate.Should().Be(shiftDate);
        result.PlannedQuantity.Should().Be(120);
        result.Results.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Employee_Not_Found()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = ShiftsTestHelper.CreateSpecification("SHIFT-SPEC");
        context.Specifications.Add(specification);
        await context.SaveChangesAsync();

        var handler = new CreateShiftPlanCommandHandler(context);

        var act = async () => await handler.Handle(
            new CreateShiftPlanCommand(Guid.NewGuid(), specification.Id, new DateOnly(2025, 3, 6), "Night", 80),
            default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Specification_Not_Found()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Create Plan Employee");
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var handler = new CreateShiftPlanCommandHandler(context);

        var act = async () => await handler.Handle(
            new CreateShiftPlanCommand(employee.Id, Guid.NewGuid(), new DateOnly(2025, 3, 7), "Night", 80),
            default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Duplicate_For_Date()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Dup Employee");
        var specification = ShiftsTestHelper.CreateSpecification("SHIFT-DUP");
        var shiftDate = new DateOnly(2025, 3, 8);

        context.Employees.Add(employee);
        context.Specifications.Add(specification);

        var plan = new ShiftPlan(employee.Id, specification.Id, shiftDate, "Day", 50);
        context.ShiftPlans.Add(plan);
        await context.SaveChangesAsync();

        var handler = new CreateShiftPlanCommandHandler(context);

        var act = async () => await handler.Handle(
            new CreateShiftPlanCommand(employee.Id, specification.Id, shiftDate, "Night", 60),
            default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
