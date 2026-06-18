using MyFactory.WebApi.Contracts.Shipments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public sealed class ShipmentDetailsResponseExample : IExamplesProvider<ShipmentDetailsResponse>
{
    public ShipmentDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        SalesOrderId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        CustomerId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
        ShipmentDate: new DateTime(2024, 5, 1),
        Status: ShipmentStatusResponse.Shipped,
        Items: new List<ShipmentDetailsItemResponse>
        {
            new(
                ShipmentItemId: Guid.Parse("55555555-5555-5555-5555-555555555555"),
                SalesOrderItemId: Guid.Parse("66666666-6666-6666-6666-666666666666"),
                ProductId: Guid.Parse("77777777-7777-7777-7777-777777777777"),
                WarehouseId: Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Qty: 10,
                UnitPrice: 1200)
        });
}
