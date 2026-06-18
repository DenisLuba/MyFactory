namespace MyFactory.MauiClient.Models.Shipments;

public sealed record ShipmentListItemResponse(
    Guid Id,
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    ShipmentStatus? Status,
    int ItemsCount,
    decimal TotalQty
);
