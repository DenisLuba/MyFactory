namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftPlanListResponse(
    Guid ShiftPlanId,
    Guid EmployeeId,
    string EmployeeName,
    Guid SpecificationId,
    string SpecificationName,
    DateTime Date,
    int PlannedQuantity
);
