using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Payroll.Commands.RecordTimesheetEntry;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Application.Tests.Features.Payroll;

public class RecordTimesheetEntryCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_TimesheetEntry()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = PayrollTestHelper.CreateEmployee("Timesheet Employee", 12m, 10m);
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var handler = new RecordTimesheetEntryCommandHandler(context);
        var workDate = new DateOnly(2025, 3, 10);

        var result = await handler.Handle(new RecordTimesheetEntryCommand(employee.Id, workDate, 8m, null), default);

        result.EmployeeId.Should().Be(employee.Id);
        result.WorkDate.Should().Be(workDate);
        result.HoursWorked.Should().Be(8m);

        var entry = await context.TimesheetEntries.SingleAsync();
        entry.EmployeeId.Should().Be(employee.Id);
        entry.HoursWorked.Should().Be(8m);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Employee_Not_Found()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var handler = new RecordTimesheetEntryCommandHandler(context);

        var act = async () => await handler.Handle(
            new RecordTimesheetEntryCommand(Guid.NewGuid(), new DateOnly(2025, 4, 1), 4m, null),
            default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Hours_Negative()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = PayrollTestHelper.CreateEmployee("Negative Hours");
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var handler = new RecordTimesheetEntryCommandHandler(context);

        var act = async () => await handler.Handle(
            new RecordTimesheetEntryCommand(employee.Id, new DateOnly(2025, 4, 2), -1m, null),
            default);

        await act.Should().ThrowAsync<DomainException>();
    }
}
