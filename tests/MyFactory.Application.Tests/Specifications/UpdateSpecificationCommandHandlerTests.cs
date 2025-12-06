using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.DTOs.Specifications;
using MyFactory.Application.Features.Specifications;
using MyFactory.Application.Features.Specifications.Commands.UpdateSpecification;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.Tests.Specifications;

public class UpdateSpecificationCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateSpecification()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var existing = new Specification("SP-001", "Original", 10m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        await context.Specifications.AddAsync(existing);
        await context.SaveChangesAsync();

        var handler = new UpdateSpecificationCommandHandler(context);
        var command = new UpdateSpecificationCommand(existing.Id, "SP-010", "Updated", 12m, "Desc");

        SpecificationMutationResultDto result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(SpecificationsStatusValues.Updated, result.Status);

        var updated = await context.Specifications.SingleAsync();
        Assert.Equal("SP-010", updated.Sku);
        Assert.Equal("Updated", updated.Name);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSpecificationMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new UpdateSpecificationCommandHandler(context);
        var command = new UpdateSpecificationCommand(Guid.NewGuid(), "SP-010", "Updated", 12m, "Desc");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSkuDuplicate()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var existing1 = new Specification("SP-001", "One", 10m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        var existing2 = new Specification("SP-002", "Two", 11m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        await context.Specifications.AddAsync(existing1);
        await context.Specifications.AddAsync(existing2);
        await context.SaveChangesAsync();

        var handler = new UpdateSpecificationCommandHandler(context);
        var command = new UpdateSpecificationCommand(existing2.Id, "SP-001", "Updated", 12m, null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
