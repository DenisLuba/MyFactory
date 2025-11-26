namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentsCreateResponse(
    Guid ShipmentId,
    ShipmentsStatus Status
);

