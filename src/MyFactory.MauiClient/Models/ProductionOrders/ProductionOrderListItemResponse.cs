using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderListItemResponse(
    Guid Id,
    string CustomerName,
    int ProductionOrderNumber,
    int SalesOrderNumber,
    string ProductName,
    decimal QtyPlanned,
    decimal QtyFinished,
    ProductionOrderStatus Status) : ListItemResponse(Id, $"№: {ProductionOrderNumber}, товар: {ProductName}"); 