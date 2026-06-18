namespace MyFactory.WebApi.Contracts.Shipments;

public sealed record UpdateShipmentRequest(
    DateTime? ShipmentDate,
    ShipmentStatusResponse? Status,
    IReadOnlyCollection<UpdateShipmentItemRequest>? Items
);
