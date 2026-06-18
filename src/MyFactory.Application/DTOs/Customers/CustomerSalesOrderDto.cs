using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.DTOs.Customers;

public sealed class CustomerSalesOrderDto
{
    public Guid Id { get; init; }
    public int OrderNumber { get; init; }
    public DateTime OrderDate { get; init; }
    public SalesOrderStatus Status { get; init; }
}
