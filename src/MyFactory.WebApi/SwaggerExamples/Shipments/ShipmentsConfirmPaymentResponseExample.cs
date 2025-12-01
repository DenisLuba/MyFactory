using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shipments;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public class ShipmentsConfirmPaymentResponseExample : IExamplesProvider<ShipmentsConfirmPaymentResponse>
{
    public ShipmentsConfirmPaymentResponse GetExamples() =>
        new(
            ShipmentId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Status: ShipmentStatus.Paid
        );
}

