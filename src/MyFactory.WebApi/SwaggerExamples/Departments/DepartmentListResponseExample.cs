using MyFactory.Domain.Entities.Organization;
using MyFactory.WebApi.Contracts.Departments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Departments;

public sealed class DepartmentListResponseExample : IExamplesProvider<IReadOnlyList<DepartmentListItemResponse>>
{
    public IReadOnlyList<DepartmentListItemResponse> GetExamples() => new List<DepartmentListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            Code: "SHW",
            Name: "Швейный цех",
            Type: DepartmentType.Production,
            IsActive: true),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            Code: "ADM",
            Name: "Администрация",
            Type: DepartmentType.Office,
            IsActive: true)
    };
}
