namespace MyFactory.MauiClient.Models.Employees;

public record EmployeeProductionAssignmentResponse(
    Guid ProductionOrderId,
    string ProductionOrderNumber,
    ProductionOrderStatus Stage,
    int QtyAssigned,
    int QtyCompleted);
