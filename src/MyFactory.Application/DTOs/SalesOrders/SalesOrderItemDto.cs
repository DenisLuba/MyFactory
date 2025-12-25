namespace MyFactory.Application.DTOs.SalesOrders;

public sealed class SalesOrderItemDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = null!;
    public decimal QtyOrdered { get; init; }
}
