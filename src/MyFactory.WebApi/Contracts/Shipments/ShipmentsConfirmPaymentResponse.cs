namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentsConfirmPaymentResponse(
    Guid ShipmentId,
    ShipmentsStatus Status
);

