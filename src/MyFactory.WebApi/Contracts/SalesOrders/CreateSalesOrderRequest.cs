namespace MyFactory.WebApi.Contracts.SalesOrders;

public sealed record CreateSalesOrderRequest(Guid CustomerId, DateTime OrderDate);
