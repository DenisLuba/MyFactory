namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record SalesOrderShipmentResponse(
    Guid Id,
    string ProductName,
    int ProductionOrderNumber,
    string WarehouseName,
    decimal Qty,
    DateTime ShippedAt);
