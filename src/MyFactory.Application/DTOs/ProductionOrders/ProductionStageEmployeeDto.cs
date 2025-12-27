namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionStageEmployeeDto
{
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = null!;
    public decimal? PlanPerHour { get; init; }
    public int AssignedQty { get; init; }
    public int CompletedQty { get; init; }
}
