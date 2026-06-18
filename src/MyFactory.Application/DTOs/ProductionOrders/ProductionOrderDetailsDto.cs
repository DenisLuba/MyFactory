using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionOrderDetailsDto
{
    public Guid Id { get; init; }
    public int ProductionOrderNumber { get; init; }
    public Guid SalesOrderId { get; init; }
    public Guid SalesOrderItemId { get; init; }
    public Guid ProductId { get; init; }
    public string? ProductName { get; init; }
    public Guid DepartmentId { get; init; }
    public string? DepartmentName { get; init; }
    public decimal QtyPlanned { get; init; }
    public decimal QtyCut { get; init; }
    public decimal QtySewn { get; init; }
    public decimal QtyPacked { get; init; }
    public decimal QtyFinished { get; init; }
    public ProductionOrderStatus Status { get; init; }
}
