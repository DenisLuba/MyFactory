using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;

namespace MyFactory.WebApi.SwaggerExamples.Shifts;

public class ShiftsGetResultsResponseExample : IExamplesProvider<ShiftsGetResultsResponse>
{
    public ShiftsGetResultsResponse GetExamples() =>
        new(
            ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            ActualQty: 14,
            HoursWorked: 7.5
        );
}

