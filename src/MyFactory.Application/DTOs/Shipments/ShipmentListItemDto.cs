namespace MyFactory.Application.DTOs.Shipments;

public sealed record ShipmentListItemDto(
    Guid Id,
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    ShipmentStatus? Status,
    int ItemsCount,
    decimal TotalQty
);