using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class UpdateProductMaterialRequestExample : IExamplesProvider<UpdateProductMaterialRequest>
{
    public UpdateProductMaterialRequest GetExamples() => new(QtyPerUnit: 1.5m);
}
