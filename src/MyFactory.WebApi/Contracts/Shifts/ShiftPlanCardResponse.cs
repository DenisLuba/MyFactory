namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftPlanCardResponse(
    Guid ShiftPlanId,
    Guid EmployeeId,
    string EmployeeName,
    Guid SpecificationId,
    string SpecificationName,
    DateTime Date,
    int PlannedQuantity
);
