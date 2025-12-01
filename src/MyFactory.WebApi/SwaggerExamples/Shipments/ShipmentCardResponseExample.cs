using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shipments;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public class ShipmentCardResponseExample : IExamplesProvider<ShipmentCardResponse>
{
    public ShipmentCardResponse GetExamples() =>
        new(
            ShipmentId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Customer: "ИП Клиент1",
            Date: new DateTime(2025, 11, 12),
            Status: ShipmentStatus.Draft,
            TotalAmount: 5500m,
            Items: new[]
            {
                new ShipmentItemDto(
                    SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    ProductName: "Пижама женская",
                    Qty: 10,
                    UnitPrice: 550.0m,
                    LineTotal: 5500m
                )
            }
        );
}
