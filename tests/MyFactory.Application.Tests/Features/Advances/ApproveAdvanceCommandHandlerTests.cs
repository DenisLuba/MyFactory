using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Advances.Commands.ApproveAdvance;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class ApproveAdvanceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldIssueDraftAdvance()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Anna Smith", "Accountant", 2, 12m, 3m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 150m, new DateOnly(2025, 3, 5));
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new ApproveAdvanceCommandHandler(context);
        var command = new ApproveAdvanceCommand(advance.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(AdvanceStatus.Issued, result.Status);
        var stored = await context.Advances.SingleAsync();
        Assert.Equal(AdvanceStatus.Issued, stored.Status);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAdvanceNotDraft()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Anna Smith", "Accountant", 2, 12m, 3m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 150m, new DateOnly(2025, 3, 5));
        advance.Issue();
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new ApproveAdvanceCommandHandler(context);
        var command = new ApproveAdvanceCommand(advance.Id);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
