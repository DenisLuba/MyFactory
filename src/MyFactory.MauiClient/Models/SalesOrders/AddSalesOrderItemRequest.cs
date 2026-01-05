namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record AddSalesOrderItemRequest(Guid ProductId, decimal Qty);
