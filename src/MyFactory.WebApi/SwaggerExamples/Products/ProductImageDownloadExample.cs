using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductImageDownloadExample : IExamplesProvider<byte[]>
{
    public byte[] GetExamples() => new byte[] { 255, 216, 255, 224 };
}
