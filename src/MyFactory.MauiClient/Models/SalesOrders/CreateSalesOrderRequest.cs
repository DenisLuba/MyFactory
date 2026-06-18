namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record CreateSalesOrderRequest(Guid CustomerId, DateTime OrderDate);
