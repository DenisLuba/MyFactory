namespace MyFactory.MauiClient.Models.Customers;

public record CustomerOrderItemResponse(
    Guid Id,
    string OrderNumber,
    DateTime OrderDate,
    SalesOrderStatus Status);
