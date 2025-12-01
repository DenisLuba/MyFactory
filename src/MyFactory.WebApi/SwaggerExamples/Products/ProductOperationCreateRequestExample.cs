using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductOperationCreateRequestExample : IExamplesProvider<ProductOperationCreateRequest>
{
    public ProductOperationCreateRequest GetExamples() => new(
        "Пошив изделия",
        35.0,
        590m
    );
}
