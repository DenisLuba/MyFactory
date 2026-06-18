using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class UpdateProductionStageEmployeeRequestExample : IExamplesProvider<UpdateProductionStageEmployeeRequest>
{
    public UpdateProductionStageEmployeeRequest GetExamples() => new(
        EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        AssignedQty: 30,
        CompletedQty: 0,
        Date: new DateOnly(2025, 3, 10));
}
