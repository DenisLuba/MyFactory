using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class AddProductionStageEmployeeRequestExample : IExamplesProvider<AddProductionStageEmployeeRequest>
{
    public AddProductionStageEmployeeRequest GetExamples() => new(
        EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        QtyPlanned: 30,
        QtyCompleted: 20,
        Date: new DateOnly(2025, 3, 10),
        HoursWorked: 6.5m);
}
