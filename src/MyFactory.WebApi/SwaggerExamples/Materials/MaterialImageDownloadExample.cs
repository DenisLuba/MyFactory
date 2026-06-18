using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public sealed class MaterialImageDownloadExample : IExamplesProvider<byte[]>
{
    public byte[] GetExamples() => [255, 216, 255, 224];
}
