using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Payroll.Commands.ClosePayrollPeriod;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using Xunit;

namespace MyFactory.Application.Tests.Features.Payroll;

public class ClosePayrollPeriodCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Close_Period_For_All_Employees()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var alice = PayrollTestHelper.CreateEmployee("Alice", 15m, 10m);
        var bob = PayrollTestHelper.CreateEmployee("Bob", 20m, 0m);
        context.Employees.AddRange(alice, bob);

        context.TimesheetEntries.AddRange(
            TimesheetEntry.Create(alice.Id, new DateOnly(2025, 3, 1), 8m, null),
            TimesheetEntry.Create(alice.Id, new DateOnly(2025, 3, 2), 6m, null),
            TimesheetEntry.Create(bob.Id, new DateOnly(2025, 3, 1), 10m, null));

        await context.SaveChangesAsync();

        var handler = new ClosePayrollPeriodCommandHandler(context);
        var summary = await handler.Handle(new ClosePayrollPeriodCommand(new DateOnly(2025, 3, 1), new DateOnly(2025, 3, 31)), default);

        summary.Entries.Should().HaveCount(2);
        summary.TotalHours.Should().Be(24m);
        summary.TotalAccruedAmount.Should().BeGreaterThan(0m);
        summary.EmployeesCount.Should().Be(2);

        var payrollEntries = await context.PayrollEntries.ToListAsync();
        payrollEntries.Should().HaveCount(2);
        payrollEntries.All(entry => entry.PeriodStart == new DateOnly(2025, 3, 1)).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_When_No_Timesheets()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var handler = new ClosePayrollPeriodCommandHandler(context);

        var summary = await handler.Handle(new ClosePayrollPeriodCommand(new DateOnly(2025, 4, 1), new DateOnly(2025, 4, 30)), default);

        summary.Entries.Should().BeEmpty();
        summary.TotalHours.Should().Be(0m);
        (await context.PayrollEntries.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Period_Overlaps()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = PayrollTestHelper.CreateEmployee("Overlap");
        context.Employees.Add(employee);
        context.PayrollEntries.Add(PayrollEntry.Create(employee.Id, new DateOnly(2025, 3, 1), new DateOnly(2025, 3, 15), 40m, 400m));
        await context.SaveChangesAsync();

        var handler = new ClosePayrollPeriodCommandHandler(context);

        var act = async () => await handler.Handle(new ClosePayrollPeriodCommand(new DateOnly(2025, 3, 10), new DateOnly(2025, 3, 31)), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_Should_Prevent_Double_Close()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = PayrollTestHelper.CreateEmployee("DoubleClose", 18m, 5m);
        context.Employees.Add(employee);
        context.TimesheetEntries.Add(TimesheetEntry.Create(employee.Id, new DateOnly(2025, 5, 1), 8m, null));
        await context.SaveChangesAsync();

        var handler = new ClosePayrollPeriodCommandHandler(context);
        await handler.Handle(new ClosePayrollPeriodCommand(new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 31)), default);

        var act = async () => await handler.Handle(new ClosePayrollPeriodCommand(new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 31)), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
