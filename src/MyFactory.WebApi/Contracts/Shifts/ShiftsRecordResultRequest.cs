namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftsRecordResultRequest(
    Guid ShiftPlanId,
    int ActualQty,
    double HoursWorked
);

