namespace MyFactory.Application.DTOs.SalesOrders;

using MyFactory.Domain.Entities.Orders;

public sealed class SalesOrderListItemDto
{
    public Guid Id { get; init; }
    public int OrderNumber { get; init; }
    public string CustomerName { get; init; } = null!;
    public DateTime OrderDate { get; init; }
    public SalesOrderStatus Status { get; init; }
}
