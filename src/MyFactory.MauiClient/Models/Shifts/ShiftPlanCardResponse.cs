namespace MyFactory.MauiClient.Models.Shifts;

public record ShiftPlanCardResponse(
    Guid ShiftPlanId,
    Guid EmployeeId,
    string EmployeeName,
    Guid SpecificationId,
    string SpecificationName,
    DateTime Date,
    int PlannedQuantity
);
