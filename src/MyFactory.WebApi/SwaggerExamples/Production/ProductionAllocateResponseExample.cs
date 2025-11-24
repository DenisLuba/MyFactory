using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionAllocateResponseExample : IExamplesProvider<ProductionAllocateResponse>
{
    public ProductionAllocateResponse GetExamples() =>
        new ProductionAllocateResponse(
            OrderId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Status: ProductionStatus.Allocated
        );
}

