namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentsGetResponse(
    Guid Id,
    string Customer,
    ShipmentItemDto[] Items
);

