using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Shifts.Commands.RecordShiftResult;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Shifts;
using Xunit;

namespace MyFactory.Application.Tests.Features.Shifts;

public class RecordShiftResultCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Add_ShiftResult()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Result Employee");
        var specification = ShiftsTestHelper.CreateSpecification("RESULT-SPEC");
        var plan = new ShiftPlan(employee.Id, specification.Id, new DateOnly(2025, 5, 1), "Day", 75);

        context.Employees.Add(employee);
        context.Specifications.Add(specification);
        context.ShiftPlans.Add(plan);
        await context.SaveChangesAsync();

        var handler = new RecordShiftResultCommandHandler(context);
        var recordedAt = DateTime.UtcNow.AddMinutes(-5);

        var result = await handler.Handle(new RecordShiftResultCommand(plan.Id, 70, 7.5m, recordedAt), default);

        result.Results.Should().ContainSingle(res => res.ActualQuantity == 70 && res.HoursWorked == 7.5m);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Plan_Not_Found()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var handler = new RecordShiftResultCommandHandler(context);

        var act = async () => await handler.Handle(
            new RecordShiftResultCommand(Guid.NewGuid(), 10, 4, DateTime.UtcNow),
            default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Hours_Invalid()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Invalid Hours Employee");
        var specification = ShiftsTestHelper.CreateSpecification("INVALID-HOURS");
        var plan = new ShiftPlan(employee.Id, specification.Id, new DateOnly(2025, 5, 2), "Day", 80);

        context.Employees.Add(employee);
        context.Specifications.Add(specification);
        context.ShiftPlans.Add(plan);
        await context.SaveChangesAsync();

        var handler = new RecordShiftResultCommandHandler(context);

        var act = async () => await handler.Handle(new RecordShiftResultCommand(plan.Id, 50, 0, DateTime.UtcNow), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_RecordedAt_In_Future()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Future Employee");
        var specification = ShiftsTestHelper.CreateSpecification("FUTURE-SPEC");
        var plan = new ShiftPlan(employee.Id, specification.Id, new DateOnly(2025, 5, 3), "Day", 60);

        context.Employees.Add(employee);
        context.Specifications.Add(specification);
        context.ShiftPlans.Add(plan);
        await context.SaveChangesAsync();

        var handler = new RecordShiftResultCommandHandler(context);

        var act = async () => await handler.Handle(
            new RecordShiftResultCommand(plan.Id, 40, 6, DateTime.UtcNow.AddMinutes(10)),
            default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
