using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class AddTimesheetEntryResponseExample : IExamplesProvider<AddTimesheetEntryResponse>
{
    public AddTimesheetEntryResponse GetExamples() => new(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0003"));
}
