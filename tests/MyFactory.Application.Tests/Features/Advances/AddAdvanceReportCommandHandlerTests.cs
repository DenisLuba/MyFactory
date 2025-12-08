using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Advances.Commands.AddAdvanceReport;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Application.Tests.Features.Advances;

public sealed class AddAdvanceReportCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddReportAndReduceBalance()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Jane Miller", "Buyer", 2, 15m, 4m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 300m, new DateOnly(2025, 5, 1));
        advance.Approve();
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new AddAdvanceReportCommandHandler(context);
        var command = new AddAdvanceReportCommand(advance.Id, "Hotel", 100m, new DateOnly(2025, 5, 2));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(200m, result.RemainingAmount);
        var reportedDto = Assert.Single(result.Reports);
        Assert.Equal(new DateOnly(2025, 5, 2), reportedDto.ReportedAt);

        var storedAdvance = await context.Advances.Include(a => a.Reports).SingleAsync();
        Assert.Equal(200m, storedAdvance.RemainingAmount);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAdvanceNotApproved()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Jane Miller", "Buyer", 2, 15m, 4m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 300m, new DateOnly(2025, 5, 1));
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new AddAdvanceReportCommandHandler(context);
        var command = new AddAdvanceReportCommand(advance.Id, "Hotel", 100m, new DateOnly(2025, 5, 2));

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAmountExceedsRemaining()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Jane Miller", "Buyer", 2, 15m, 4m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 300m, new DateOnly(2025, 5, 1));
        advance.Approve();
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new AddAdvanceReportCommandHandler(context);
        var command = new AddAdvanceReportCommand(advance.Id, "Hotel", 400m, new DateOnly(2025, 5, 2));

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }
}
