using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Advances.Commands.RejectAdvance;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class RejectAdvanceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldMarkAdvanceAsRejected()
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

        Assert.Equal(AdvanceStatuses.Rejected, result.Status);
        var stored = await context.Advances.SingleAsync();
        Assert.Equal(AdvanceStatuses.Rejected, stored.Status);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAdvanceNotDraft()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Paul Brown", "Manager", 3, 20m, 10m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 200m, new DateOnly(2025, 4, 10));
        advance.Approve();
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new RejectAdvanceCommandHandler(context);
        var command = new RejectAdvanceCommand(advance.Id);

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }
}
