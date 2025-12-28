using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.Organization;

public sealed class EmployeeProductionAssignmentDto
{
    public Guid ProductionOrderId { get; init; }
    public string ProductionOrderNumber { get; init; } = null!;

    public ProductionOrderStatus Stage { get; init; }

    public int QtyPlanned { get; init; }
    public int QtyCompleted { get; init; }
}