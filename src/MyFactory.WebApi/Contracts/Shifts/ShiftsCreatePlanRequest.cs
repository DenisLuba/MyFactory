namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftsCreatePlanRequest(
    Guid EmployeeId,
    Guid SpecificationId,
    int PlannedQuantity,
    DateTime Date
);

