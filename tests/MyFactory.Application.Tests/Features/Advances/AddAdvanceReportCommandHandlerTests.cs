using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Advances.Commands.AddAdvanceReport;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Files;
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
        advance.Issue();
        await context.Advances.AddAsync(advance);
        var file = new FileResource("receipt.pdf", "/files/receipt.pdf", "application/pdf", 1024, Guid.NewGuid(), DateTime.UtcNow);
        await context.FileResources.AddAsync(file);
        await context.SaveChangesAsync();

        var handler = new AddAdvanceReportCommandHandler(context);
        var command = new AddAdvanceReportCommand(advance.Id, "Hotel", 100m, file.Id, new DateOnly(2025, 5, 2));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(200m, result.RemainingAmount);
        Assert.Single(result.Reports);

        var storedAdvance = await context.Advances.Include(a => a.Reports).SingleAsync();
        Assert.Equal(200m, storedAdvance.RemainingAmount);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenFileMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Jane Miller", "Buyer", 2, 15m, 4m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 300m, new DateOnly(2025, 5, 1));
        advance.Issue();
        await context.Advances.AddAsync(advance);
        await context.SaveChangesAsync();

        var handler = new AddAdvanceReportCommandHandler(context);
        var command = new AddAdvanceReportCommand(advance.Id, "Hotel", 100m, Guid.NewGuid(), new DateOnly(2025, 5, 2));

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAmountExceedsRemaining()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var employee = new Employee("Jane Miller", "Buyer", 2, 15m, 4m);
        await context.Employees.AddAsync(employee);
        var advance = new Advance(employee.Id, 300m, new DateOnly(2025, 5, 1));
        advance.Issue();
        await context.Advances.AddAsync(advance);
        var file = new FileResource("receipt.pdf", "/files/receipt.pdf", "application/pdf", 1024, Guid.NewGuid(), DateTime.UtcNow);
        await context.FileResources.AddAsync(file);
        await context.SaveChangesAsync();

        var handler = new AddAdvanceReportCommandHandler(context);
        var command = new AddAdvanceReportCommand(advance.Id, "Hotel", 400m, file.Id, new DateOnly(2025, 5, 2));

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
