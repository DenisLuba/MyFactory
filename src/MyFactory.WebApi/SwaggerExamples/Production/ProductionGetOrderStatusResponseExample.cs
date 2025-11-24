using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionGetOrderStatusResponseExample : IExamplesProvider<ProductionGetOrderStatusResponse>
{
    public ProductionGetOrderStatusResponse GetExamples() =>
        new ProductionGetOrderStatusResponse(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Status: ProductionStatus.Allocated
        );
}

