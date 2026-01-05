using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductImageResponseExample : IExamplesProvider<IReadOnlyList<ProductImageResponse>>
{
    public IReadOnlyList<ProductImageResponse> GetExamples() => new List<ProductImageResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            ProductId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            FileName: "front.jpg",
            Path: "products/bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002/front.jpg",
            ContentType: "image/jpeg",
            SortOrder: 1),
        new(
            Id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
            ProductId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            FileName: "back.jpg",
            Path: "products/bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002/back.jpg",
            ContentType: "image/jpeg",
            SortOrder: 2)
    };
}
