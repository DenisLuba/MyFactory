namespace MyFactory.WebApi.Contracts.SalesOrders;

public sealed record UpdateSalesOrderRequest(Guid CustomerId, DateTime OrderDate);
