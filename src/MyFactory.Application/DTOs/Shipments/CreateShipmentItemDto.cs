namespace MyFactory.Application.DTOs.Shipments;

public sealed record CreateShipmentItemDto(
    Guid SalesOrderItemId,
    Guid ProductId,
    Guid WarehouseId,
    int Qty,
    decimal UnitPrice
);
