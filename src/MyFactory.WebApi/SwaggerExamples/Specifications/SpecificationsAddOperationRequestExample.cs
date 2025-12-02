using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsAddOperationRequestExample : IExamplesProvider<SpecificationsAddOperationRequest>
{
    public SpecificationsAddOperationRequest GetExamples() =>
        new(
            OperationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
            Minutes: 120.0,
            Cost: 150.0m
        );
}
