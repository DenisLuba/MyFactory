using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class TimesheetListResponseExample : IExamplesProvider<IReadOnlyList<TimesheetListItemResponse>>
{
    public IReadOnlyList<TimesheetListItemResponse> GetExamples() => new List<TimesheetListItemResponse>
    {
        new(
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            EmployeeName: "Иванов Иван",
            DepartmentName: "Швейный цех",
            TotalHours: 160,
            WorkDays: 20),
        new(
            EmployeeId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            EmployeeName: "Петров Петр",
            DepartmentName: "Раскрой",
            TotalHours: 152,
            WorkDays: 19)
    };
}
