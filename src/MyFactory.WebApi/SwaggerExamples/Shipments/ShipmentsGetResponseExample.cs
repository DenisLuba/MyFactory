using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shipments;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public class ShipmentsGetResponseExample : IExamplesProvider<ShipmentsGetResponse>
{
    public ShipmentsGetResponse GetExamples() =>
        new(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Customer: "ИП Клиент1",
            Items: new[]
            {
                new ShipmentItemDto(
                    SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Qty: 10,
                    UnitPrice: 550.0m
                )
            }
        );
}

