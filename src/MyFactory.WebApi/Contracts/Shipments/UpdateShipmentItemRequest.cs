namespace MyFactory.WebApi.Contracts.Shipments;

public sealed record UpdateShipmentItemRequest(
    Guid SalesOrderItemId,
    Guid ProductId,
    Guid WarehouseId,
    int Qty,
    decimal UnitPrice
);
