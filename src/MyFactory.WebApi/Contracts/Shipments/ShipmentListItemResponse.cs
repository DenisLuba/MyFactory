namespace MyFactory.WebApi.Contracts.Shipments;

public sealed record ShipmentListItemResponse(
    Guid Id,
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    ShipmentStatusResponse? Status,
    int ItemsCount,
    decimal TotalQty
);
