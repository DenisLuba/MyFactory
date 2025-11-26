namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftsGetResultsResponse(
    Guid ShiftPlanId,
    int ActualQty,
    double HoursWorked
);

