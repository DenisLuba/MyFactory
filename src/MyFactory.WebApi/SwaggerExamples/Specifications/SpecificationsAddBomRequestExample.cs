using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsAddBomRequestExample : IExamplesProvider<SpecificationsAddBomRequest>
{
    public SpecificationsAddBomRequest GetExamples() =>
        new(
            MaterialId: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
            Qty: 255,
            Unit: "м.",
            Price: 120.0m
        );
}
