using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class CreateProductionOrderRequestExample : IExamplesProvider<CreateProductionOrderRequest>
{
    public CreateProductionOrderRequest GetExamples() => new(
        SalesOrderItemId: Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeee0001"),
        DepartmentId: Guid.Parse("11111111-2222-3333-4444-555555555555"),
        QtyPlanned: 120);
}
