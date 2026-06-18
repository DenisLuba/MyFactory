using MyFactory.WebApi.Contracts.ProductTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductTypes;

public sealed class ProductTypeDetailsResponseExample : IExamplesProvider<ProductTypeResponse>
{
    public ProductTypeResponse GetExamples() => new(
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        "Футболки",
        "Базовые футболки"
    );
}
