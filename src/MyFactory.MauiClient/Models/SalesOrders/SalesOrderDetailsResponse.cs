using MyFactory.MauiClient.Models.SalesOrders;

namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record SalesOrderDetailsResponse(
    Guid Id,
    string OrderNumber,
    DateTime OrderDate,
    SalesOrderStatus Status,
    CustomerDetailsResponse Customer,
    IReadOnlyList<SalesOrderItemResponse> Items);
