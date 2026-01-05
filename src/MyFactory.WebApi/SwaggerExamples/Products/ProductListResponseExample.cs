using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductListResponseExample : IExamplesProvider<IReadOnlyList<ProductListItemResponse>>
{
    public IReadOnlyList<ProductListItemResponse> GetExamples() => new List<ProductListItemResponse>
    {
        new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), "Пижама женская", 520m),
        new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"), "Халат махровый", 310m)
    };
}
