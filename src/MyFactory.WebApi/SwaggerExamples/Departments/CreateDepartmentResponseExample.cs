using MyFactory.WebApi.Contracts.Departments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Departments;

public sealed class CreateDepartmentResponseExample : IExamplesProvider<CreateDepartmentResponse>
{
    public CreateDepartmentResponse GetExamples() => new(
        Id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"));
}
