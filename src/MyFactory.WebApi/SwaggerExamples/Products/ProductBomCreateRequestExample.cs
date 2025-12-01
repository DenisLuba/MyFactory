using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductBomCreateRequestExample : IExamplesProvider<ProductBomCreateRequest>
{
    public ProductBomCreateRequest GetExamples() => new(
        "Ткань хлопок",
        2.4,
        "м",
        380m
    );
}
