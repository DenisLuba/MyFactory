namespace MyFactory.MauiClient.Models.Shipments;

public record ShipmentsConfirmPaymentResponse(
    Guid ShipmentId,
    ShipmentsStatus Status
);

