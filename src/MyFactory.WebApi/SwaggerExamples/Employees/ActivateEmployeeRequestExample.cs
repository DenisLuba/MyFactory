using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class ActivateEmployeeRequestExample : IExamplesProvider<ActivateEmployeeRequest>
{
    public ActivateEmployeeRequest GetExamples() => new(new DateTime(2024, 1, 10));
}
