using MyFactory.WebApi.Contracts.Shipments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public sealed class UpdateShipmentRequestExample : IExamplesProvider<UpdateShipmentRequest>
{
    public UpdateShipmentRequest GetExamples() => new(
        ShipmentDate: new DateTime(2024, 5, 2),
        Status: ShipmentStatusResponse.Confirmed,
        Items: new List<UpdateShipmentItemRequest>
        {
            new(
                SalesOrderItemId: Guid.Parse("66666666-6666-6666-6666-666666666666"),
                ProductId: Guid.Parse("77777777-7777-7777-7777-777777777777"),
                WarehouseId: Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Qty: 8,
                UnitPrice: 1200)
        });
}
