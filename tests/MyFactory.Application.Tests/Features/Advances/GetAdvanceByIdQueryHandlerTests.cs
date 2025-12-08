using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Advances.Queries.GetAdvanceById;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class GetAdvanceByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAdvanceWithReports()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Caleb Fry", "Engineer", 2, 18m, 5m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 90m, new DateOnly(2025, 7, 1));
        advance.Approve();
        await context.Advances.AddAsync(advance);
        var report = advance.AddReport("Meal", 30m, new DateOnly(2025, 7, 2), Guid.NewGuid(), new DateOnly(2025, 7, 2));
        await context.AdvanceReports.AddAsync(report);
        await context.SaveChangesAsync();

        var handler = new GetAdvanceByIdQueryHandler(context);
        var result = await handler.Handle(new GetAdvanceByIdQuery(advance.Id), CancellationToken.None);

        Assert.Equal(advance.Id, result.Id);
        Assert.Single(result.Reports);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAdvanceMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetAdvanceByIdQueryHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetAdvanceByIdQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
