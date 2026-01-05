using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class SalesOrderShipmentsResponseExample : IExamplesProvider<IReadOnlyList<SalesOrderShipmentResponse>>
{
    public IReadOnlyList<SalesOrderShipmentResponse> GetExamples() => new List<SalesOrderShipmentResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            ProductName: "Пижама женская",
            ProductionOrderNumber: "PO-001",
            WarehouseName: "Готовая продукция",
            Qty: 40m,
            ShippedAt: new DateTime(2025, 3, 20)),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            ProductName: "Пижама женская",
            ProductionOrderNumber: "PO-001",
            WarehouseName: "Готовая продукция",
            Qty: 80m,
            ShippedAt: new DateTime(2025, 3, 22))
    };
}
