using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionGetOrderResponseExample : IExamplesProvider<ProductionGetOrderResponse>
{
    public ProductionGetOrderResponse GetExamples() =>
        new ProductionGetOrderResponse(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            QtyOrdered: 10,
            Allocation: new[]
            {
                new Allocation(
                    WorkshopId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    QtyAllocated: 8
                )
            }
        );
}

