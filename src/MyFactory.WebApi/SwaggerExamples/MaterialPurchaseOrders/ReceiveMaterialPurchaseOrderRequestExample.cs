using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class ReceiveMaterialPurchaseOrderRequestExample : IExamplesProvider<ReceiveMaterialPurchaseOrderRequest>
{
    public ReceiveMaterialPurchaseOrderRequest GetExamples() => new(
        WarehouseId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"),
        ReceivedByUserId: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0005"));
}
