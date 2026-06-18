namespace MyFactory.MauiClient.Models.Shipments;

public sealed record UpdateShipmentRequest(
    DateTime? ShipmentDate,
    ShipmentStatus? Status,
    IReadOnlyCollection<UpdateShipmentItemRequest>? Items
);
