using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionStageAssignmentDto
{
    public Guid AssignmentId { get; init; }
    public ProductionStage Stage { get; init; }
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = null!;
    public decimal? PlanPerHour { get; init; }
    public decimal AssignedQty { get; init; }
    public decimal CompletedQty { get; init; }
    public DateOnly WorkDate { get; init; }
}

