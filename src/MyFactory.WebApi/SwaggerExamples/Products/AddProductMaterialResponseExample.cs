using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class AddProductMaterialResponseExample : IExamplesProvider<AddProductMaterialResponse>
{
    public AddProductMaterialResponse GetExamples() => new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0003"));
}
