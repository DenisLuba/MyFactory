using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductImageFilesResponseExample : IExamplesProvider<IReadOnlyList<ProductImageFileResponse>>
{
    public IReadOnlyList<ProductImageFileResponse> GetExamples() => new List<ProductImageFileResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            ProductId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            FileName: "front.jpg",
            ContentType: "image/jpeg",
            Content: new byte[] { 255, 216, 255, 224 }),
        new(
            Id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
            ProductId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            FileName: "back.jpg",
            ContentType: "image/jpeg",
            Content: new byte[] { 255, 216, 255, 225 })
    };
}
