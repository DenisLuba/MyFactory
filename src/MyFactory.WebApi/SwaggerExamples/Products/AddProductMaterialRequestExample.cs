using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class AddProductMaterialRequestExample : IExamplesProvider<AddProductMaterialRequest>
{
    public AddProductMaterialRequest GetExamples() => new(
        MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"),
        QtyPerUnit: 1.2m);
}
