namespace MyFactory.MauiClient.Models.Shipments;

public sealed record CreateShipmentRequest(
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    Guid CreatedBy,
    ShipmentStatus? Status,
    IReadOnlyCollection<CreateShipmentItemRequest> Items
);
