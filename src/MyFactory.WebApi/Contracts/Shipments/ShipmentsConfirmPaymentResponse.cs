using System;

namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentsConfirmPaymentResponse(
    Guid ShipmentId,
    ShipmentStatus Status
);

