using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionCreateOrderResponseExample : IExamplesProvider<ProductionCreateOrderResponse>
{
    public ProductionCreateOrderResponse GetExamples() =>
        new ProductionCreateOrderResponse(
            OrderId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Status: ProductionStatus.Created
        );
}

