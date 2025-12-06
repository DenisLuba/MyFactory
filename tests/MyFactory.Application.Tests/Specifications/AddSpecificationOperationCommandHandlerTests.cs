using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.Features.Specifications;
using MyFactory.Application.Features.Specifications.Commands.AddOperation;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Operations;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Application.Tests.Specifications;

public class AddSpecificationOperationCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddOperation()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var specification = new Specification("SP-001", "Robe", 5m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        var operation = new Operation("CUT", "Cutting", 5m, 10m, "Cut");
        var workshop = new Workshop("Main", "Sewing");

        await context.Specifications.AddAsync(specification);
        await context.Operations.AddAsync(operation);
        await context.Workshops.AddAsync(workshop);
        await context.SaveChangesAsync();

        var handler = new AddSpecificationOperationCommandHandler(context);
        var command = new AddSpecificationOperationCommand(specification.Id, operation.Id, workshop.Id, 6m, 12m);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(SpecificationsStatusValues.OperationAdded, result.Status);
        Assert.Single(await context.SpecificationOperations.ToListAsync());
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenWorkshopInactive()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var specification = new Specification("SP-001", "Robe", 5m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        var operation = new Operation("CUT", "Cutting", 5m, 10m, "Cut");
        var workshop = new Workshop("Main", "Sewing");
        workshop.Deactivate();

        await context.Specifications.AddAsync(specification);
        await context.Operations.AddAsync(operation);
        await context.Workshops.AddAsync(workshop);
        await context.SaveChangesAsync();

        var handler = new AddSpecificationOperationCommandHandler(context);
        var command = new AddSpecificationOperationCommand(specification.Id, operation.Id, workshop.Id, 6m, 12m);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSpecificationMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var operation = new Operation("CUT", "Cutting", 5m, 10m, "Cut");
        var workshop = new Workshop("Main", "Sewing");
        await context.Operations.AddAsync(operation);
        await context.Workshops.AddAsync(workshop);
        await context.SaveChangesAsync();

        var handler = new AddSpecificationOperationCommandHandler(context);
        var command = new AddSpecificationOperationCommand(Guid.NewGuid(), operation.Id, workshop.Id, 6m, 12m);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
