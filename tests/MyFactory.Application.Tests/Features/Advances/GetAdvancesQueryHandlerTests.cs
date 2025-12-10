using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Advances.Queries.GetAdvances;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class GetAdvancesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldFilterByStatus()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Laura Hill", "Planner", 2, 16m, 4m);
        await context.Employees.AddAsync(employee);
        var draft = new Advance(employee.Id, 80m, new DateOnly(2025, 8, 1));
        var approved = new Advance(employee.Id, 120m, new DateOnly(2025, 8, 2));
        approved.Approve();
        await context.Advances.AddRangeAsync(draft, approved);
        await context.SaveChangesAsync();

        var handler = new GetAdvancesQueryHandler(context);
        var result = await handler.Handle(new GetAdvancesQuery(AdvanceStatuses.Approved, null), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(AdvanceStatuses.Approved, result.Single().Status);
    }

    [Fact]
    public async Task Handle_ShouldFilterByEmployee()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee1 = new Employee("Laura Hill", "Planner", 2, 16m, 4m);
        var employee2 = new Employee("Greg Cole", "Planner", 2, 16m, 4m);
        await context.Employees.AddRangeAsync(employee1, employee2);
        var advance1 = new Advance(employee1.Id, 80m, new DateOnly(2025, 8, 1));
        var advance2 = new Advance(employee2.Id, 120m, new DateOnly(2025, 8, 2));
        await context.Advances.AddRangeAsync(advance1, advance2);
        await context.SaveChangesAsync();

        var handler = new GetAdvancesQueryHandler(context);
        var result = await handler.Handle(new GetAdvancesQuery(null, employee2.Id), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(employee2.Id, result.Single().EmployeeId);
    }
}
