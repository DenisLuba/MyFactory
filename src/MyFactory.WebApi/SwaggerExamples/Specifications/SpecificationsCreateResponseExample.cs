using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsCreateResponseExample : IExamplesProvider<SpecificationsCreateResponse>
{
    public SpecificationsCreateResponse GetExamples() =>
        new
        (
            Id: Guid.NewGuid(),
            Status: SpecificationsStatus.Created
        );
}
