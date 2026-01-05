using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class CreateEmployeeResponseExample : IExamplesProvider<CreateEmployeeResponse>
{
    public CreateEmployeeResponse GetExamples() => new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"));
}
