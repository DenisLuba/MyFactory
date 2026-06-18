using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductImageDownloadExample : IExamplesProvider<byte[]>
{
    public byte[] GetExamples() => [255, 216, 255, 224];
}
