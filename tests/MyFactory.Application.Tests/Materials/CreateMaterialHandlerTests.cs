using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.Features.Materials.Commands.CreateMaterial;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Tests.Materials;

public class CreateMaterialHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateMaterial_WhenDataValid()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Metals");
        await context.MaterialTypes.AddAsync(type);
        await context.SaveChangesAsync();

        var handler = new CreateMaterialCommandHandler(context);
        var command = new CreateMaterialCommand("Steel", type.Id, "kg");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Steel", result.Name);
        Assert.Equal("kg", result.Unit);
        Assert.Equal(type.Id, result.MaterialType.Id);
        Assert.Equal(1, await context.Materials.CountAsync());
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMaterialTypeMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new CreateMaterialCommandHandler(context);
        var command = new CreateMaterialCommand("Steel", Guid.NewGuid(), "kg");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenNameDuplicates()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Metals");
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(new Material("Steel", type.Id, "kg"));
        await context.SaveChangesAsync();

        var handler = new CreateMaterialCommandHandler(context);
        var command = new CreateMaterialCommand("Steel", type.Id, "kg");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
