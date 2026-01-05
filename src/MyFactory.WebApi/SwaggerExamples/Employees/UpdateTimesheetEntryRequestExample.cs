using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class UpdateTimesheetEntryRequestExample : IExamplesProvider<UpdateTimesheetEntryRequest>
{
    public UpdateTimesheetEntryRequest GetExamples() => new(
        Hours: 7.5m,
        Comment: "Сокращённый день");
}
