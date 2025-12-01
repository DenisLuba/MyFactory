using System;

namespace MyFactory.MauiClient.Models.Shipments;

public record ShipmentsConfirmPaymentResponse(
    Guid ShipmentId,
    ShipmentStatus Status
);

