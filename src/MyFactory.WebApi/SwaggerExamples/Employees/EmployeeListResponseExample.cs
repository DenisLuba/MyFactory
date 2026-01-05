using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class EmployeeListResponseExample : IExamplesProvider<IReadOnlyList<EmployeeListItemResponse>>
{
    public IReadOnlyList<EmployeeListItemResponse> GetExamples() => new List<EmployeeListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            FullName: "Иванов Иван Иванович",
            DepartmentName: "Швейный цех",
            PositionName: "Швея",
            IsActive: true),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            FullName: "Петров Петр Петрович",
            DepartmentName: "Раскрой",
            PositionName: "Закройщик",
            IsActive: false)
    };
}
