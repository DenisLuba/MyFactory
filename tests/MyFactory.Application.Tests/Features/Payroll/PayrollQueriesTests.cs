using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Payroll.Commands.ClosePayrollPeriod;
using MyFactory.Application.Features.Payroll.Queries.GetPayrollForPeriod;
using MyFactory.Application.Features.Payroll.Queries.GetTimesheetEntriesByEmployee;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using Xunit;

namespace MyFactory.Application.Tests.Features.Payroll;

public class PayrollQueriesTests
{
    [Fact]
    public async Task GetTimesheetEntriesByEmployee_Should_Filter_By_Date()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = PayrollTestHelper.CreateEmployee("Query Employee");
        context.Employees.Add(employee);
        context.TimesheetEntries.AddRange(
            TimesheetEntry.Create(employee.Id, new DateOnly(2025, 6, 1), 4m, null),
            TimesheetEntry.Create(employee.Id, new DateOnly(2025, 6, 5), 6m, null),
            TimesheetEntry.Create(employee.Id, new DateOnly(2025, 7, 1), 8m, null));
        await context.SaveChangesAsync();

        var handler = new GetTimesheetEntriesByEmployeeQueryHandler(context);
        var entries = await handler.Handle(
            new GetTimesheetEntriesByEmployeeQuery(employee.Id, new DateOnly(2025, 6, 2), new DateOnly(2025, 6, 30)),
            default);

        entries.Should().HaveCount(1);
        entries.Single().HoursWorked.Should().Be(6m);
    }

    [Fact]
    public async Task GetPayrollForPeriod_Should_Return_Entries()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = PayrollTestHelper.CreateEmployee("Payroll Query", 25m, 20m);
        context.Employees.Add(employee);
        context.TimesheetEntries.Add(TimesheetEntry.Create(employee.Id, new DateOnly(2025, 8, 1), 8m, null));
        await context.SaveChangesAsync();

        var closeHandler = new ClosePayrollPeriodCommandHandler(context);
        await closeHandler.Handle(new ClosePayrollPeriodCommand(new DateOnly(2025, 8, 1), new DateOnly(2025, 8, 31)), default);

        var handler = new GetPayrollForPeriodQueryHandler(context);
        var entries = await handler.Handle(new GetPayrollForPeriodQuery(new DateOnly(2025, 8, 1), new DateOnly(2025, 8, 31)), default);

        entries.Should().HaveCount(1);
        entries.Single().EmployeeId.Should().Be(employee.Id);
        entries.Single().TotalHours.Should().Be(8m);
    }
}
