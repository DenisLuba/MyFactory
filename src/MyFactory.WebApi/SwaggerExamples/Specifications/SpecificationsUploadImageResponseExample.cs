using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsUploadImageResponseExample : IExamplesProvider<SpecificationsUploadImageResponse>
{
    public SpecificationsUploadImageResponse GetExamples() =>
        new(
            "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
            SpecificationsStatus.ImageUploaded
        );
}
