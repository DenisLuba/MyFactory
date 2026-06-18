using MyFactory.Domain.Entities.Orders;

namespace MyFactory.WebApi.Contracts.SalesOrders;

public sealed record SalesOrderListItemResponse(
    Guid Id,
    int OrderNumber,
    string CustomerName,
    DateTime OrderDate,
    SalesOrderStatus Status);
