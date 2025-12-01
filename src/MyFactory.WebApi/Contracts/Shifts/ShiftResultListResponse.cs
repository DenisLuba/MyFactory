namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftResultListResponse(
    Guid ShiftPlanId,
    string EmployeeName,
    string SpecificationName,
    DateTime Date,
    int PlannedQuantity,
    int ActualQty,
    double HoursWorked,
    bool Bonus
);
