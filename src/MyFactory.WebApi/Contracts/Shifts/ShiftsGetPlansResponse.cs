namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftsGetPlansResponse(
    Guid ShiftPlanId,
    Guid EmployeeId,
    Guid SpecificationId,
    int PlannedQuantity,
    DateTime Date
);

