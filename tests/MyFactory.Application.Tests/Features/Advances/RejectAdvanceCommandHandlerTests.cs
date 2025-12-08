using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Advances.Commands.RejectAdvance;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class RejectAdvanceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRemoveDraftAdvance()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Paul Brown", "Manager", 3, 20m, 10m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 200m, new DateOnly(2025, 4, 10));
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new RejectAdvanceCommandHandler(context);
        var command = new RejectAdvanceCommand(advance.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(AdvanceStatus.Draft, result.Status);
        Assert.Empty(await context.Advances.ToListAsync());
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAdvanceNotDraft()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Paul Brown", "Manager", 3, 20m, 10m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 200m, new DateOnly(2025, 4, 10));
        advance.Issue();
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new RejectAdvanceCommandHandler(context);
        var command = new RejectAdvanceCommand(advance.Id);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
