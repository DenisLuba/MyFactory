using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.Features.Specifications;
using MyFactory.Application.Features.Specifications.Commands.DeleteBomItem;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.Tests.Specifications;

public class DeleteSpecificationBomItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteBomItem()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var (specification, bomItem) = await SeedSpecificationWithBomAsync(context);
        var handler = new DeleteSpecificationBomItemCommandHandler(context);
        var command = new DeleteSpecificationBomItemCommand(specification.Id, bomItem.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(SpecificationsStatusValues.BomDeleted, result.Status);
        Assert.Empty(await context.SpecificationBomItems.ToListAsync());
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenBomItemMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var specification = new Specification("SP-001", "Robe", 5m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        await context.Specifications.AddAsync(specification);
        await context.SaveChangesAsync();

        var handler = new DeleteSpecificationBomItemCommandHandler(context);
        var command = new DeleteSpecificationBomItemCommand(specification.Id, Guid.NewGuid());

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    private static async Task<(Specification Specification, SpecificationBomItem BomItem)> SeedSpecificationWithBomAsync(TestApplicationDbContext context)
    {
        var type = new MaterialType("Fabric");
        var material = new Material("Cotton", type.Id, "m");
        var specification = new Specification("SP-001", "Robe", 5m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        var bomItem = specification.AddBomItem(material.Id, 1m, "m", 3m);

        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.Specifications.AddAsync(specification);
        await context.SpecificationBomItems.AddAsync(bomItem);
        await context.SaveChangesAsync();

        return (specification, bomItem);
    }
}
