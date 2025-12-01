using MyFactory.WebApi.Contracts.Operations;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Operations;

public class OperationUpdateRequestExample : IExamplesProvider<OperationUpdateRequest>
{
    public OperationUpdateRequest GetExamples() => new(
        Code: "OPR-002",
        Name: "Пошив основы",
        OperationType: "Пошив",
        Minutes: 34.0,
        Cost: 500.0m
    );
}
