using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.Employees;

public sealed class EmployeeProductionAssignmentDto
{
    public Guid ProductionOrderId { get; init; }
    public int ProductionOrderNumber { get; init; }

    public ProductionOrderStatus Stage { get; init; }

    public decimal QtyAssigned { get; init; }
    public decimal QtyCompleted { get; init; }
}