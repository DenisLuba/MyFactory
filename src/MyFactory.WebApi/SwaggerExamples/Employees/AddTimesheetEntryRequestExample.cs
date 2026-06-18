using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class AddTimesheetEntryRequestExample : IExamplesProvider<AddTimesheetEntryRequest>
{
    public AddTimesheetEntryRequest GetExamples() => new(
        Date: new DateOnly(2025, 3, 5),
        Hours: 8,
        Comment: "Смена 1");
}
