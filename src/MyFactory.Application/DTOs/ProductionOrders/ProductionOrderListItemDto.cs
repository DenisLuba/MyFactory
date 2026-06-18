using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionOrderListItemDto
{
    public Guid Id { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public int ProductionOrderNumber { get; init; }
    public int SalesOrderNumber { get; init; }
    public string ProductName { get; init; } = null!;
    public decimal QtyPlanned { get; init; }
    public decimal QtyFinished { get; init; }
    public ProductionOrderStatus Status { get; init; }
}