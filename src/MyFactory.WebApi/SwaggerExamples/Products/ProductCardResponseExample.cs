using System;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductCardResponseExample : IExamplesProvider<ProductCardResponse>
{
    public ProductCardResponse GetExamples() => new(
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        "SP-001",
        "Пижама женская",
        2.5,
        "Лёгкая летняя пижама",
        2
    );
}
