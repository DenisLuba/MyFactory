namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record SalesOrderItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal QtyOrdered,
    decimal QtyAllocated,
    decimal QtyShipped);
