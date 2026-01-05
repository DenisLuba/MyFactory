using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class EmployeeTimesheetDetailsExample : IExamplesProvider<IReadOnlyList<EmployeeTimesheetEntryResponse>>
{
    public IReadOnlyList<EmployeeTimesheetEntryResponse> GetExamples() => new List<EmployeeTimesheetEntryResponse>
    {
        new(
            EntryId: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0001"),
            Date: new DateOnly(2025, 3, 1),
            Hours: 8,
            Comment: null),
        new(
            EntryId: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0002"),
            Date: new DateOnly(2025, 3, 2),
            Hours: 7.5m,
            Comment: "Отпуск 0.5ч")
    };
}
