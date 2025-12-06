using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.Features.Materials.Commands.UpdateMaterial;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Tests.Materials;

public class UpdateMaterialHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateMaterial()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var originalType = new MaterialType("Metals");
        var newType = new MaterialType("Plastics");
        await context.MaterialTypes.AddRangeAsync(originalType, newType);

        var material = new Material("Steel", originalType.Id, "kg");
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();

        var handler = new UpdateMaterialCommandHandler(context);
        var command = new UpdateMaterialCommand(material.Id, "Stainless Steel", newType.Id, "ton", false);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Stainless Steel", result.Name);
        Assert.Equal("ton", result.Unit);
        Assert.False(result.IsActive);
        Assert.Equal(newType.Id, result.MaterialType.Id);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMaterialNotFound()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new UpdateMaterialCommandHandler(context);
        var command = new UpdateMaterialCommand(Guid.NewGuid(), "Name", null, null, null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMaterialTypeMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Metals");
        await context.MaterialTypes.AddAsync(type);
        var material = new Material("Steel", type.Id, "kg");
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();

        var handler = new UpdateMaterialCommandHandler(context);
        var command = new UpdateMaterialCommand(material.Id, null, Guid.NewGuid(), null, null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenReactivatingMaterial()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Metals");
        await context.MaterialTypes.AddAsync(type);
        var material = new Material("Steel", type.Id, "kg");
        material.Deactivate();
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();

        var handler = new UpdateMaterialCommandHandler(context);
        var command = new UpdateMaterialCommand(material.Id, null, null, null, true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
