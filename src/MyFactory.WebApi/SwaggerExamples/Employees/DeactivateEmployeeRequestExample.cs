using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class DeactivateEmployeeRequestExample : IExamplesProvider<DeactivateEmployeeRequest>
{
    public DeactivateEmployeeRequest GetExamples() => new(new DateTime(2024, 2, 15));
}
