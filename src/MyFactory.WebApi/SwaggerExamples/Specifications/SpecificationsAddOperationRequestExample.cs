using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsAddOperationRequestExample : IExamplesProvider<SpecificationsAddOperationRequest>
{
    public SpecificationsAddOperationRequest GetExamples() =>
        new(
            Code: "SPC-009",
            Name: "Пошив молнии",
            Minutes: 120.0,
            Cost: 150.0m
        );
}
