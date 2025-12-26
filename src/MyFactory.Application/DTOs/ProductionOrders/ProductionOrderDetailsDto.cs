using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionOrderDetailsDto
{
    public Guid Id { get; init; }
    public string ProductionOrderNumber { get; init; } = null!;
    public Guid SalesOrderItemId { get; init; }
    public Guid DepartmentId { get; init; }
    public int QtyPlanned { get; init; }
    public int QtyCut { get; init; }
    public int QtySewn { get; init; }
    public int QtyPacked { get; init; }
    public int QtyFinished { get; init; }
    public ProductionOrderStatus Status { get; init; }
}
