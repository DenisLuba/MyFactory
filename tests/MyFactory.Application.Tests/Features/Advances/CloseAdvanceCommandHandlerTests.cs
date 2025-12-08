using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Advances.Commands.CloseAdvance;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class CloseAdvanceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCloseAdvanceWithZeroBalance()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Mary Lee", "Supervisor", 4, 25m, 8m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 150m, new DateOnly(2025, 6, 1));
        advance.Approve();
        await context.Advances.AddAsync(advance);
        var report = advance.AddReport("Taxi", 150m, new DateOnly(2025, 6, 2));
        await context.AdvanceReports.AddAsync(report);
        await context.SaveChangesAsync();

        var handler = new CloseAdvanceCommandHandler(context);
        var closedAt = new DateOnly(2025, 6, 3);
        var command = new CloseAdvanceCommand(advance.Id, closedAt);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(AdvanceStatus.Closed, result.Status);
        Assert.Equal(closedAt, result.ClosedAt);
        var stored = await context.Advances.SingleAsync();
        Assert.Equal(AdvanceStatus.Closed, stored.Status);
        Assert.Equal(closedAt, stored.ClosedAt);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenBalanceRemains()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Mary Lee", "Supervisor", 4, 25m, 8m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 150m, new DateOnly(2025, 6, 1));
        advance.Approve();
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new CloseAdvanceCommandHandler(context);
        var command = new CloseAdvanceCommand(advance.Id, new DateOnly(2025, 6, 5));

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }
}
