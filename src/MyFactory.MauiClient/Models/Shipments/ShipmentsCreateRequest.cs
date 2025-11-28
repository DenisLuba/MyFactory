namespace MyFactory.MauiClient.Models.Shipments;

public record ShipmentsCreateRequest(
    Guid CustomerId,
    ShipmentItemDto[] Items
);

public record ShipmentItemDto(
    Guid SpecificationId,
    int Qty,
    decimal UnitPrice
);

