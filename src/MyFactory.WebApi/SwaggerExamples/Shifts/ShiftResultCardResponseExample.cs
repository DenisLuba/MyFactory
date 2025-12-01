using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;

namespace MyFactory.WebApi.SwaggerExamples.Shifts;

public class ShiftResultCardResponseExample : IExamplesProvider<ShiftResultCardResponse>
{
    public ShiftResultCardResponse GetExamples() =>
        new(
            ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            EmployeeName: "Иванова О.Г.",
            SpecificationName: "Пижама женская",
            Date: new DateTime(2025, 12, 12),
            PlannedQty: 12,
            ActualQty: 14,
            HoursWorked: 7.5,
            Bonus: true
        );
}
