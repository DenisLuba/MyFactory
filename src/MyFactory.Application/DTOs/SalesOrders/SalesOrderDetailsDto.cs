using MyFactory.Application.DTOs.Customers;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.DTOs.SalesOrders;

public sealed class SalesOrderDetailsDto
{
    public Guid Id { get; init; }
    public string OrderNumber { get; init; } = null!;
    public DateTime OrderDate { get; init; }
    public SalesOrderStatus Status { get; init; }
    public CustomerDetailsDto Customer { get; init; } = null!;
    public IReadOnlyList<SalesOrderItemDto> Items { get; init; } = null!;
}
