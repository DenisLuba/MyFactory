using MyFactory.Domain.Entities.Organization;
using MyFactory.WebApi.Contracts.Departments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Departments;

public sealed class UpdateDepartmentRequestExample : IExamplesProvider<UpdateDepartmentRequest>
{
    public UpdateDepartmentRequest GetExamples() => new(
        Name: "Ўвейный цех",
        Code: "SHW",
        Type: DepartmentType.Production,
        IsActive: true);
}
