using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsAddOperationResponseExample : IExamplesProvider<SpecificationsAddOperationResponse>
{
    public SpecificationsAddOperationResponse GetExamples() =>
        new(
            SpecificationId: id,
            OperationId: Guid.NewGuid(),
            Status: SpecificationsStatus.OperationAdded
        );
}
