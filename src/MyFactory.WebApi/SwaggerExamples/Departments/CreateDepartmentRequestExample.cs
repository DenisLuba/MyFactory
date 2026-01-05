using MyFactory.Domain.Entities.Organization;
using MyFactory.WebApi.Contracts.Departments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Departments;

public sealed class CreateDepartmentRequestExample : IExamplesProvider<CreateDepartmentRequest>
{
    public CreateDepartmentRequest GetExamples() => new(
        Name: "Ўвейный цех",
        Code: "SHW",
        Type: DepartmentType.Production);
}
