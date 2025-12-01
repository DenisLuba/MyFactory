using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public class ProductionOrderDeleteResponseExample : IExamplesProvider<ProductionOrderDeleteResponse>
{
    public ProductionOrderDeleteResponse GetExamples() =>
        new(
            OrderId: Guid.Parse("10000000-0000-0000-0000-000000000002"),
            IsDeleted: true);
}
