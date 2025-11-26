using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsUploadImageResponseExample : IExamplesProvider<SpecificationsUploadImageResponse>
{
    public SpecificationsUploadImageResponse GetExamples() =>
        new(
            SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Status: SpecificationsStatus.ImageUploaded
        );
}
