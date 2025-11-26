namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftsRecordResultResponse(
    Guid ShiftPlanId,
    ShiftsStatus Status
);

