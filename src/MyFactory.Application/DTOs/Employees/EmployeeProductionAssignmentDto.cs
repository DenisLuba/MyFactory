using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.Employees;

public sealed class EmployeeProductionAssignmentDto
{
    public Guid ProductionOrderId { get; init; }
    public string ProductionOrderNumber { get; init; } = null!;

    public ProductionOrderStatus Stage { get; init; }

    public int QtyAssigned { get; init; }
    public int QtyCompleted { get; init; }
}