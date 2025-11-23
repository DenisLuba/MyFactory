using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Materials;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class AddMaterialPriceResponseExample : IExamplesProvider<AddMaterialPriceResponse>
{
    public AddMaterialPriceResponse GetExamples() =>
        new(
            Status : MaterialPriceStatus.PriceAdded,
            Id : Guid.Parse("22222222-2222-2222-2222-222222222222")
        );
}
