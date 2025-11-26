using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shipments;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public class ShipmentsCreateRequestExample : IExamplesProvider<ShipmentsCreateRequest>
{
    public ShipmentsCreateRequest GetExamples() =>
        new(
            CustomerId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
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

