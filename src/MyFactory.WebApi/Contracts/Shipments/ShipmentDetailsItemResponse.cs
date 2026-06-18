namespace MyFactory.WebApi.Contracts.Shipments;

public sealed record ShipmentDetailsItemResponse(
    Guid ShipmentItemId,
    Guid SalesOrderItemId,
    Guid ProductId,
    Guid WarehouseId,
    int Qty,
    decimal UnitPrice
);
