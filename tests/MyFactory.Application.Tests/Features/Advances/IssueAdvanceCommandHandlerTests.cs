using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Advances.Commands.IssueAdvance;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class IssueAdvanceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateDraftAdvance()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("John Doe", "Operator", 1, 10m, 5m);
        await context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();

        var handler = new IssueAdvanceCommandHandler(context);
        var command = new IssueAdvanceCommand(employee.Id, 100m, new DateOnly(2025, 1, 1));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(employee.Id, result.EmployeeId);
        Assert.Equal(100m, result.Amount);
        Assert.Equal(AdvanceStatus.Draft, result.Status);

        var stored = await context.Advances.SingleAsync();
        Assert.Equal(AdvanceStatus.Draft, stored.Status);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenEmployeeMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new IssueAdvanceCommandHandler(context);
        var command = new IssueAdvanceCommand(Guid.NewGuid(), 50m, new DateOnly(2025, 2, 1));

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
