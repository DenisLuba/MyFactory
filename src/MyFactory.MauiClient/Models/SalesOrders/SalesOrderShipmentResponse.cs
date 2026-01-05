namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record SalesOrderShipmentResponse(
    Guid Id,
    string ProductName,
    string ProductionOrderNumber,
    string WarehouseName,
    decimal Qty,
    DateTime ShippedAt);
