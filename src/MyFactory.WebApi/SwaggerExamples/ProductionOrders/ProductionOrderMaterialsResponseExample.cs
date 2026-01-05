using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionOrderMaterialsResponseExample : IExamplesProvider<IReadOnlyList<ProductionOrderMaterialResponse>>
{
    public IReadOnlyList<ProductionOrderMaterialResponse> GetExamples() => new List<ProductionOrderMaterialResponse>
    {
        new(
            MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            MaterialName: "Ситец",
            RequiredQty: 120,
            AvailableQty: 80,
            MissingQty: 40),
        new(
            MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            MaterialName: "Молния 20 см",
            RequiredQty: 120,
            AvailableQty: 200,
            MissingQty: 0)
    };
}
