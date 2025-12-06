using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Materials.Queries.GetMaterials;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using Xunit;

namespace MyFactory.Application.Tests.Materials;

public class GetMaterialsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllMaterials()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var metals = new MaterialType("Metals");
        var plastics = new MaterialType("Plastics");
        await context.MaterialTypes.AddRangeAsync(metals, plastics);
        await context.Materials.AddRangeAsync(
            new Material("Steel", metals.Id, "kg"),
            new Material("Polymer", plastics.Id, "kg"));
        await context.SaveChangesAsync();

        var handler = new GetMaterialsQueryHandler(context);
        var result = await handler.Handle(new GetMaterialsQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, dto => dto.MaterialType.Name == "Metals");
    }
}
