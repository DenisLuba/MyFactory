using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;

namespace MyFactory.WebApi.SwaggerExamples.Shifts;

public class ShiftsRecordResultRequestExample : IExamplesProvider<ShiftsRecordResultRequest>
{
    public ShiftsRecordResultRequest GetExamples() =>
        new(
            ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            ActualQty: 14,
            HoursWorked: 7.5,
            Bonus: true
        );
}

