using System;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductBomDeleteResponseExample : IExamplesProvider<ProductBomDeleteResponse>
{
    public ProductBomDeleteResponse GetExamples() => new(
        Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
        "Deleted"
    );
}
