using MyFactory.WebApi.Contracts.Shipments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public sealed class CreateShipmentRequestExample : IExamplesProvider<CreateShipmentRequest>
{
    public CreateShipmentRequest GetExamples() => new(
        SalesOrderId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        CustomerId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
        ShipmentDate: new DateTime(2024, 5, 1),
        CreatedBy: Guid.Parse("99999999-9999-9999-9999-999999999999"),
        Status: ShipmentStatusResponse.Confirmed,
        Items: new List<CreateShipmentItemRequest>
        {
            new(
                SalesOrderItemId: Guid.Parse("66666666-6666-6666-6666-666666666666"),
                ProductId: Guid.Parse("77777777-7777-7777-7777-777777777777"),
                WarehouseId: Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Qty: 10,
                UnitPrice: 1200)
        });
}
