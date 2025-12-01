using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductOperationItemResponseExample : IExamplesProvider<IEnumerable<ProductOperationItemResponse>>
{
    public IEnumerable<ProductOperationItemResponse> GetExamples() => new[]
    {
        new ProductOperationItemResponse(
            Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
            "Раскрой комплектов",
            18.5,
            265m
        ),
        new ProductOperationItemResponse(
            Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"),
            "Пошив изделия",
            42.5,
            590m
        )
    };
}
