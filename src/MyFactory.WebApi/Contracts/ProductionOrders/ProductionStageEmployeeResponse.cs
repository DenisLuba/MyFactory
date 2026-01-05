namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionStageEmployeeResponse(
    Guid EmployeeId,
    string EmployeeName,
    decimal? PlanPerHour,
    int AssignedQty,
    int CompletedQty);
