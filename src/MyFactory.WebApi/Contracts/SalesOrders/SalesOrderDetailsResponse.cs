using MyFactory.Domain.Entities.Orders;

namespace MyFactory.WebApi.Contracts.SalesOrders;

public sealed record SalesOrderDetailsResponse(
    Guid Id,
    string OrderNumber,
    DateTime OrderDate,
    SalesOrderStatus Status,
    CustomerDetailsResponse Customer,
    IReadOnlyList<SalesOrderItemResponse> Items);
