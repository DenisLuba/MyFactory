using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;

namespace MyFactory.WebApi.SwaggerExamples.Shifts;

public class ShiftPlanListResponseExample : IExamplesProvider<IEnumerable<ShiftPlanListResponse>>
{
    public IEnumerable<ShiftPlanListResponse> GetExamples() =>
        new[]
        {
            new ShiftPlanListResponse(
                ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                EmployeeName: "Иванова О.Г.",
                SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                SpecificationName: "Пижама женская",
                Date: new DateTime(2025, 12, 12),
                PlannedQuantity: 12
            ),
            new ShiftPlanListResponse(
                ShiftPlanId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                EmployeeId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                EmployeeName: "Сергейчук А.А.",
                SpecificationId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                SpecificationName: "Пижама женская",
                Date: new DateTime(2025, 12, 12),
                PlannedQuantity: 15
            )
        };
}

