using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class UpdateProductionOrderRequestExample : IExamplesProvider<UpdateProductionOrderRequest>
{
    public UpdateProductionOrderRequest GetExamples() => new(
        DepartmentId: Guid.Parse("11111111-2222-3333-4444-555555555555"),
        QtyPlanned: 140);
}
