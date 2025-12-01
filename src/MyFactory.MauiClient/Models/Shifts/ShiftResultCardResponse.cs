namespace MyFactory.MauiClient.Models.Shifts;

public record ShiftResultCardResponse(
    Guid ShiftPlanId,
    string EmployeeName,
    string SpecificationName,
    DateTime Date,
    int PlannedQty,
    int ActualQty,
    double HoursWorked,
    bool Bonus
);
