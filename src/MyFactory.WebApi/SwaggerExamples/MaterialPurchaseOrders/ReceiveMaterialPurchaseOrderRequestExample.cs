using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class ReceiveMaterialPurchaseOrderRequestExample : IExamplesProvider<ReceiveMaterialPurchaseOrderRequest>
{
    public ReceiveMaterialPurchaseOrderRequest GetExamples() => new(
        ReceivedByUserId: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0005"),
        Allocations: new List<ReceiveMaterialPurchaseOrderAllocationRequest>
        {
            new(
                WarehouseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                ShippingCost: 100m,
                MaterialItems: new List<ReceiveMaterialPurchaseOrderItemRequest>
                {
                    new(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0001"), 10m, 2m),
                }
            ),
            new(
                WarehouseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"),
                ShippingCost: 100m,
                MaterialItems: new List<ReceiveMaterialPurchaseOrderItemRequest>
                {
                    new(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"), 20m)
                }
            )
        });
}
