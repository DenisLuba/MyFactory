using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsAddOperationResponseExample : IExamplesProvider<SpecificationsAddOperationResponse>
{
    public SpecificationsAddOperationResponse GetExamples() =>
        new(
            SpecificationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Item: new SpecificationOperationItemResponse(
                Id: Guid.NewGuid(),
                Operation: "Сборка",
                Minutes: 12,
                Cost: 48
            ),
            Status: SpecificationsStatus.OperationAdded
        );
}
