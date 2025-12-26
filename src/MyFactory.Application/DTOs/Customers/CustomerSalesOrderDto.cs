using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.DTOs.Customers;

public sealed class CustomerSalesOrderDto
{
    public Guid Id { get; init; }
    public string OrderNumber { get; init; } = null!;
    public DateTime OrderDate { get; init; }
    public SalesOrderStatus Status { get; init; }
}
