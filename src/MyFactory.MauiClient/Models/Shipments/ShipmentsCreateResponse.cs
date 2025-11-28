namespace MyFactory.MauiClient.Models.Shipments;

public record ShipmentsCreateResponse(
    Guid ShipmentId,
    ShipmentsStatus Status
);

