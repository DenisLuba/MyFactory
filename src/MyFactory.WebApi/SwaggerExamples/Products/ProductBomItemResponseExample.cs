using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductBomItemResponseExample : IExamplesProvider<IEnumerable<ProductBomItemResponse>>
{
    public IEnumerable<ProductBomItemResponse> GetExamples() => new[]
    {
        new ProductBomItemResponse(
            Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
            "Ткань хлопок",
            2.4,
            "м",
            380m,
            912m
        ),
        new ProductBomItemResponse(
            Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
            "Резинка эластичная",
            1.2,
            "м",
            52m,
            62.4m
        )
    };
}
