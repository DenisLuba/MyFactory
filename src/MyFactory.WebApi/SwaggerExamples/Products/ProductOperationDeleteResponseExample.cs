using System;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductOperationDeleteResponseExample : IExamplesProvider<ProductOperationDeleteResponse>
{
    public ProductOperationDeleteResponse GetExamples() => new(
        Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
        "Deleted"
    );
}
