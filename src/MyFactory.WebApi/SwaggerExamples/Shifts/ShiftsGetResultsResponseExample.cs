using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;

namespace MyFactory.WebApi.SwaggerExamples.Shifts;

public class ShiftResultListResponseExample : IExamplesProvider<IEnumerable<ShiftResultListResponse>>
{
    public IEnumerable<ShiftResultListResponse> GetExamples() =>
        new[]
        {
            new ShiftResultListResponse(
                ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                EmployeeName: "Иванова О.Г.",
                SpecificationName: "Пижама женская",
                Date: new DateTime(2025, 12, 12),
                PlannedQuantity: 12,
                ActualQty: 14,
                HoursWorked: 7.5,
                Bonus: true
            ),
            new ShiftResultListResponse(
                ShiftPlanId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                EmployeeName: "Сергейчук А.А.",
                SpecificationName: "Пижама женская",
                Date: new DateTime(2025, 12, 12),
                PlannedQuantity: 15,
                ActualQty: 15,
                HoursWorked: 8,
                Bonus: false
            )
        };
}

