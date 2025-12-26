using MyFactory.Domain.Entities.Production;

public sealed class ProductionOrderListItemDto
{
    public Guid Id { get; init; }
    public string ProductionOrderNumber { get; init; } = null!;
    public string SalesOrderNumber { get; init; } = null!;
    public string ProductName { get; init; } = null!;
    public int QtyPlanned { get; init; }
    public int QtyFinished { get; init; }
    public ProductionOrderStatus Status { get; init; }
}
