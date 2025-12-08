using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Advances.Queries.GetAdvanceReports;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class GetAdvanceReportsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnReportsForAdvance()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Eric West", "Analyst", 1, 14m, 2m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 60m, new DateOnly(2025, 9, 1));
        advance.Issue();
        await context.Advances.AddAsync(advance);
        var report = advance.AddReport("Fuel", 20m, Guid.NewGuid(), new DateOnly(2025, 9, 2));
        await context.AdvanceReports.AddAsync(report);
        await context.SaveChangesAsync();

        var handler = new GetAdvanceReportsQueryHandler(context);
        var result = await handler.Handle(new GetAdvanceReportsQuery(advance.Id), CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAdvanceMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetAdvanceReportsQueryHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetAdvanceReportsQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
