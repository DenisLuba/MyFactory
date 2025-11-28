namespace MyFactory.MauiClient.Models.Shipments;

public record ShipmentsGetResponse(
    Guid Id,
    string Customer,
    ShipmentItemDto[] Items
);

