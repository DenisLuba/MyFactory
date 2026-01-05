using MyFactory.MauiClient.Models.ProductionOrders;

namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderListItemResponse(
    Guid Id,
    string ProductionOrderNumber,
    string SalesOrderNumber,
    string ProductName,
    int QtyPlanned,
    int QtyFinished,
    ProductionOrderStatus Status);
