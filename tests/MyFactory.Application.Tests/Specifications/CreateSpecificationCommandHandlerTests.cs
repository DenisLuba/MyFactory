using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.DTOs.Specifications;
using MyFactory.Application.Features.Specifications.Commands.CreateSpecification;
using MyFactory.Application.Features.Specifications;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.Tests.Specifications;

public class CreateSpecificationCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateSpecification()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new CreateSpecificationCommandHandler(context);
        var command = new CreateSpecificationCommand("SP-001", "Jersey", 12.5m, "Warm");

        SpecificationMutationResultDto result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(SpecificationsStatusValues.Created, result.Status);
        Assert.NotEqual(Guid.Empty, result.SpecificationId);

        var specification = await context.Specifications.SingleAsync();
        Assert.Equal("SP-001", specification.Sku);
        Assert.Equal("Jersey", specification.Name);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSkuExists()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var existing = new Specification("SP-001", "Existing", 10m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        await context.Specifications.AddAsync(existing);
        await context.SaveChangesAsync();

        var handler = new CreateSpecificationCommandHandler(context);
        var command = new CreateSpecificationCommand("SP-001", "Duplicate", 5m, null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
