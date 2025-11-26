using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;

namespace MyFactory.WebApi.SwaggerExamples.Shifts;

public class ShiftsCreatePlanRequestExample : IExamplesProvider<ShiftsCreatePlanRequest>
{
    public ShiftsCreatePlanRequest GetExamples() =>
        new(
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            PlannedQuantity: 12,
            Date: new DateTime(2025, 12, 12)
        );
}

