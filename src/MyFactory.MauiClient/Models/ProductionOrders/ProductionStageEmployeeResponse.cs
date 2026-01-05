namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionStageEmployeeResponse(
    Guid EmployeeId,
    string EmployeeName,
    decimal? PlanPerHour,
    int AssignedQty,
    int CompletedQty);
