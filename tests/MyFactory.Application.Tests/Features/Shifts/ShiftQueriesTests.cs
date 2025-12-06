using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Shifts.Queries.GetShiftPlanById;
using MyFactory.Application.Features.Shifts.Queries.GetShiftPlansByDate;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Shifts;
using Xunit;

namespace MyFactory.Application.Tests.Features.Shifts;

public class ShiftQueriesTests
{
    [Fact]
    public async Task GetShiftPlanById_Should_Return_Plan_With_Results()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employee = ShiftsTestHelper.CreateEmployee("Query Employee");
        var specification = ShiftsTestHelper.CreateSpecification("QUERY-SPEC");
        var plan = new ShiftPlan(employee.Id, specification.Id, new DateOnly(2025, 6, 1), "Day", 100);
        var result = new ShiftResult(plan.Id, 95, 7, DateTime.UtcNow.AddMinutes(-30));

        context.Employees.Add(employee);
        context.Specifications.Add(specification);
        context.ShiftPlans.Add(plan);
        context.ShiftResults.Add(result);
        await context.SaveChangesAsync();

        var handler = new GetShiftPlanByIdQueryHandler(context);

        var dto = await handler.Handle(new GetShiftPlanByIdQuery(plan.Id), default);

        dto.Id.Should().Be(plan.Id);
        dto.Results.Should().ContainSingle(r => r.ActualQuantity == 95 && r.HoursWorked == 7);
    }

    [Fact]
    public async Task GetShiftPlansByDate_Should_Filter_By_Employee()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var employeeA = ShiftsTestHelper.CreateEmployee("Employee A");
        var employeeB = ShiftsTestHelper.CreateEmployee("Employee B");
        var specification = ShiftsTestHelper.CreateSpecification("QUERY-LIST");
        var date = new DateOnly(2025, 6, 2);

        var planA = new ShiftPlan(employeeA.Id, specification.Id, date, "Day", 80);
        var planB = new ShiftPlan(employeeB.Id, specification.Id, date, "Night", 90);

        context.Employees.AddRange(employeeA, employeeB);
        context.Specifications.Add(specification);
        context.ShiftPlans.AddRange(planA, planB);
        await context.SaveChangesAsync();

        var handler = new GetShiftPlansByDateQueryHandler(context);

        var dtos = await handler.Handle(new GetShiftPlansByDateQuery(date, employeeA.Id), default);

        dtos.Should().ContainSingle(plan => plan.EmployeeId == employeeA.Id);
        dtos.Should().NotContain(plan => plan.EmployeeId == employeeB.Id);
    }
}
