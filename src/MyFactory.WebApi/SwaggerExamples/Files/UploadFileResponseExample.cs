

using MyFactory.WebApi.Contracts.Files;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Files;

public class UploadFileResponseExample : IExamplesProvider<UploadFileResponse>
{
    public UploadFileResponse GetExamples() =>
        new(
            FileId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FileName: "image.jpg"
        );
}