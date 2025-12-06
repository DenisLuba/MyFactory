using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Materials.Queries.GetMaterialById;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using Xunit;

namespace MyFactory.Application.Tests.Materials;

public class GetMaterialByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMaterial()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Metals");
        await context.MaterialTypes.AddAsync(type);
        var material = new Material("Steel", type.Id, "kg");
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();

        var handler = new GetMaterialByIdQueryHandler(context);
        var result = await handler.Handle(new GetMaterialByIdQuery(material.Id), CancellationToken.None);

        Assert.Equal(material.Id, result.Id);
        Assert.Equal(type.Name, result.MaterialType.Name);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMaterialMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetMaterialByIdQueryHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetMaterialByIdQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
