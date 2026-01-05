using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionOrderShipmentsResponseExample : IExamplesProvider<IReadOnlyList<ProductionOrderShipmentResponse>>
{
    public IReadOnlyList<ProductionOrderShipmentResponse> GetExamples() => new List<ProductionOrderShipmentResponse>
    {
        new(
            WarehouseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            WarehouseName: "Склад отгрузки",
            Qty: 30,
            ShipmentDate: new DateTime(2025, 3, 10)),
        new(
            WarehouseId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            WarehouseName: "Склад готовой продукции",
            Qty: 20,
            ShipmentDate: new DateTime(2025, 3, 12))
    };
}
