using MyFactory.MauiClient.Models.Customers;

namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record SalesOrderDetailsResponse(
    Guid Id,
    int OrderNumber,
    DateTime OrderDate,
    SalesOrderStatus Status,
    CustomerDetailsResponse Customer,
    IReadOnlyList<SalesOrderItemResponse> Items);
