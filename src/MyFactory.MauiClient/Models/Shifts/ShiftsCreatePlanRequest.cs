namespace MyFactory.MauiClient.Models.Shifts;

public record ShiftsCreatePlanRequest(
    Guid EmployeeId,
    Guid SpecificationId,
    int PlannedQuantity,
    DateTime Date
);

