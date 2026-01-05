using MyFactory.MauiClient.Models.SalesOrders;

namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record SalesOrderListItemResponse(
    Guid Id,
    string OrderNumber,
    string CustomerName,
    DateTime OrderDate,
    SalesOrderStatus Status);
