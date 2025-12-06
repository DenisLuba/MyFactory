using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.Features.Specifications;
using MyFactory.Application.Features.Specifications.Commands.AddBomItem;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.Tests.Specifications;

public class AddSpecificationBomItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddBomItem()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var (specification, material) = await SeedSpecificationWithMaterialAsync(context);
        var handler = new AddSpecificationBomItemCommandHandler(context);
        var command = new AddSpecificationBomItemCommand(specification.Id, material.Id, 2m, "m", 5m);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(SpecificationsStatusValues.BomAdded, result.Status);
        Assert.Single(await context.SpecificationBomItems.ToListAsync());
        Assert.Equal(10m, result.Item.Cost);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSpecificationMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Fabric");
        var material = new Material("Cotton", type.Id, "m");
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();

        var handler = new AddSpecificationBomItemCommandHandler(context);
        var command = new AddSpecificationBomItemCommand(Guid.NewGuid(), material.Id, 1m, "m", 2m);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMaterialMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var specification = new Specification("SP-001", "Robe", 5m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);
        await context.Specifications.AddAsync(specification);
        await context.SaveChangesAsync();

        var handler = new AddSpecificationBomItemCommandHandler(context);
        var command = new AddSpecificationBomItemCommand(specification.Id, Guid.NewGuid(), 1m, "m", 2m);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    private static async Task<(Specification Specification, Material Material)> SeedSpecificationWithMaterialAsync(TestApplicationDbContext context)
    {
        var type = new MaterialType("Fabric");
        var material = new Material("Cotton", type.Id, "m");
        var specification = new Specification("SP-001", "Robe", 5m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);

        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.Specifications.AddAsync(specification);
        await context.SaveChangesAsync();
        context.Entry(specification).Collection(s => s.BomItems).Load();
        return (specification, material);
    }
}
