using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shipments;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public class ShipmentsListResponseExample : IExamplesProvider<IEnumerable<ShipmentsListResponse>>
{
    public IEnumerable<ShipmentsListResponse> GetExamples() =>
        new[]
        {
            new ShipmentsListResponse(
                ShipmentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Customer: "ИП Клиент1",
                ProductName: "Пижама женская",
                Quantity: 10,
                Date: new DateTime(2025, 11, 12),
                TotalAmount: 5500m,
                Status: ShipmentStatus.Draft
            ),
            new ShipmentsListResponse(
                ShipmentId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Customer: "ООО \"Текстиль\"",
                ProductName: "Футболка детская",
                Quantity: 25,
                Date: new DateTime(2025, 11, 15),
                TotalAmount: 4250m,
                Status: ShipmentStatus.Paid
            )
        };
}
