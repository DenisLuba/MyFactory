using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ShipFinishedGoodsRequestExample : IExamplesProvider<ShipFinishedGoodsRequest>
{
    public ShipFinishedGoodsRequest GetExamples() => new(
        FromWarehouseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        ToWarehouseId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
        Qty: 50);
}
