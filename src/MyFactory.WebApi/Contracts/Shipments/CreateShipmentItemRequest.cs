namespace MyFactory.WebApi.Contracts.Shipments;

public sealed record CreateShipmentItemRequest(
    Guid SalesOrderItemId,
    Guid ProductId,
    Guid WarehouseId,
    int Qty,
    decimal UnitPrice
);
