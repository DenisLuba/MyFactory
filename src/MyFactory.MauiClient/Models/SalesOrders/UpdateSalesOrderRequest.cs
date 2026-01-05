namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record UpdateSalesOrderRequest(Guid CustomerId, DateTime OrderDate);
