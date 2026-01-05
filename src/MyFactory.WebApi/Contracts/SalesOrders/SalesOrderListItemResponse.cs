using MyFactory.Domain.Entities.Orders;

namespace MyFactory.WebApi.Contracts.SalesOrders;

public sealed record SalesOrderListItemResponse(
    Guid Id,
    string OrderNumber,
    string CustomerName,
    DateTime OrderDate,
    SalesOrderStatus Status);
