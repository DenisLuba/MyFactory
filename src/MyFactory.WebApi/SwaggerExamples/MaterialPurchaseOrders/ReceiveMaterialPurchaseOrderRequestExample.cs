using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class ReceiveMaterialPurchaseOrderRequestExample : IExamplesProvider<ReceiveMaterialPurchaseOrderRequest>
{
    public ReceiveMaterialPurchaseOrderRequest GetExamples() => new(
        ReceivedByUserId: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0005"),
        Items: new List<ReceiveMaterialPurchaseOrderItemRequest>
        {
            new(
                ItemId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                Allocations: new List<ReceiveMaterialPurchaseOrderAllocationRequest>
                {
                    new(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"), 50m)
                }
            ),
            new(
                ItemId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"),
                Allocations: new List<ReceiveMaterialPurchaseOrderAllocationRequest>
                {
                    new(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"), 20m)
                }
            )
        });
}
