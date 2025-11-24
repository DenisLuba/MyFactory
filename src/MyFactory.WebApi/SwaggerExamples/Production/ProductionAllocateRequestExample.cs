using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionAllocateRequestExample : IExamplesProvider<ProductionAllocateRequest>
{
    public ProductionAllocateRequest GetExamples() =>
        new ProductionAllocateRequest(
            Allocations: new[]
            {
                new Allocation(
                    WorkshopId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    QtyAllocated: 8
                )
            }
        );
}

