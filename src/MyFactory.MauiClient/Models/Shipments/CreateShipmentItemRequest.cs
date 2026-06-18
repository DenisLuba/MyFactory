namespace MyFactory.MauiClient.Models.Shipments;

public sealed record CreateShipmentItemRequest(
    Guid SalesOrderItemId,
    Guid ProductId,
    Guid WarehouseId,
    int Qty,
    decimal UnitPrice
);
