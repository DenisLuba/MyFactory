using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class UpdateProductionStageEmployeeRequestExample : IExamplesProvider<UpdateProductionStageEmployeeRequest>
{
    public UpdateProductionStageEmployeeRequest GetExamples() => new(
        EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        QtyPlanned: 40,
        Qty: 25,
        Date: new DateOnly(2025, 3, 12),
        HoursWorked: 7.0m);
}
