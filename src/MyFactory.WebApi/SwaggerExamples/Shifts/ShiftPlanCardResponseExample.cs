using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;

namespace MyFactory.WebApi.SwaggerExamples.Shifts;

public class ShiftPlanCardResponseExample : IExamplesProvider<ShiftPlanCardResponse>
{
    public ShiftPlanCardResponse GetExamples() =>
        new(
            ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            EmployeeName: "Иванова О.Г.",
            SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            SpecificationName: "Пижама женская",
            Date: new DateTime(2025, 12, 12),
            PlannedQuantity: 12
        );
}
