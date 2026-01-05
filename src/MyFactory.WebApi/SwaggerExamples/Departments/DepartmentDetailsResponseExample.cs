using MyFactory.Domain.Entities.Organization;
using MyFactory.WebApi.Contracts.Departments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Departments;

public sealed class DepartmentDetailsResponseExample : IExamplesProvider<DepartmentDetailsResponse>
{
    public DepartmentDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Code: "SHW",
        Name: "Ўвейный цех",
        Type: DepartmentType.Production,
        IsActive: true);
}
