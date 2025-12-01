using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public class ProductionOrderUpdateResponseExample : IExamplesProvider<ProductionOrderUpdateResponse>
{
    public ProductionOrderUpdateResponse GetExamples() =>
        new(
            OrderId: Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Status: "В работе");
}
