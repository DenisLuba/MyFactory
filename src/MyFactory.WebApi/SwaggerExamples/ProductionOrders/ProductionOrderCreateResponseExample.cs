using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public class ProductionOrderCreateResponseExample : IExamplesProvider<ProductionOrderCreateResponse>
{
    public ProductionOrderCreateResponse GetExamples() =>
        new(
            OrderId: Guid.Parse("20000000-0000-0000-0000-000000000001"),
            OrderNumber: "PO-003",
            Status: "Черновик");
}
