namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionStageAssignmentResponse(
    Guid AssignmentId,
    ProductionStage Stage,
    Guid EmployeeId,
    string EmployeeName,
    decimal? PlanPerHour,
    decimal AssignedQty,
    decimal CompletedQty,
    DateOnly WorkDate);
