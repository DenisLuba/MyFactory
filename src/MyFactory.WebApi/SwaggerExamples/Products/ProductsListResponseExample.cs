using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductsListResponseExample : IExamplesProvider<IEnumerable<ProductsListResponse>>
{
    public IEnumerable<ProductsListResponse> GetExamples() => new[]
    {
        new ProductsListResponse(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "SP-001",
            "Пижама женская",
            2.5,
            "Активен",
            2
        ),
        new ProductsListResponse(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "DR-215",
            "Платье трикотажное",
            3.2,
            "Черновик",
            3
        )
    };
}
