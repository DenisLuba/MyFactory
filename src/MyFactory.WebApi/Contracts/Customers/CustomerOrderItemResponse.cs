using MyFactory.Domain.Entities.Orders;

namespace MyFactory.WebApi.Contracts.Customers;

public record CustomerOrderItemResponse(
    Guid Id,
    string OrderNumber,
    DateTime OrderDate,
    SalesOrderStatus Status);
