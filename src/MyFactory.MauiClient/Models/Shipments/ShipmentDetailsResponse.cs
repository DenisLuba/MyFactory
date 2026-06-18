namespace MyFactory.MauiClient.Models.Shipments;

public sealed record ShipmentDetailsResponse(
    Guid Id,
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    ShipmentStatus? Status,
    IReadOnlyCollection<ShipmentDetailsItemResponse> Items
);
