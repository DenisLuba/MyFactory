using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Materials;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class AddMaterialPriceRequestExample : IExamplesProvider<AddMaterialPriceRequest>
{
    public AddMaterialPriceRequest GetExamples() =>
        new(
            SupplierId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            MaterialPrice: 180.0m,
            EffectiveFrom: new DateTime(2025, 11, 1)
        );
}
