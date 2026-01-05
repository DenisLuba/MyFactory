namespace MyFactory.WebApi.Contracts.SalesOrders;

public sealed record AddSalesOrderItemRequest(Guid ProductId, decimal Qty);
