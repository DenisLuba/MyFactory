using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class MaterialPriceHistoryResponseExample : IExamplesProvider<MaterialPriceHistoryResponse>
{
    public MaterialPriceHistoryResponse GetExamples() =>
        new(
            MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            SupplierId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Price: 175.50m,
            EffectiveFrom: new DateTime(2025, 11, 1)
        );
}
