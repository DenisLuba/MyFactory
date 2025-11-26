using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;

namespace MyFactory.WebApi.SwaggerExamples.Shifts;

public class ShiftsCreatePlanResponseExample : IExamplesProvider<ShiftsCreatePlanResponse>
{
    public ShiftsCreatePlanResponse GetExamples() =>
        new(
            ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Status: ShiftsStatus.Created
        );
}

