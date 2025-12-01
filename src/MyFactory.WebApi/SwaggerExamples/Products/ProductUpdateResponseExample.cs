using System;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductUpdateResponseExample : IExamplesProvider<ProductUpdateResponse>
{
    public ProductUpdateResponse GetExamples() => new(
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        "Updated"
    );
}
