namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentsCreateRequest(
    Guid CustomerId,
    ShipmentItemDto[] Items
);

public record ShipmentItemDto(
    Guid SpecificationId,
    int Qty,
    decimal UnitPrice
);

