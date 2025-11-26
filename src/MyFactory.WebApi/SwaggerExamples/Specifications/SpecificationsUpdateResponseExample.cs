using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsUpdateResponseExample : IExamplesProvider<SpecificationsUpdateResponse>
{
    public SpecificationsUpdateResponse GetExamples() =>
        new(
            "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
            SpecificationsStatus.Updated
        );
}
