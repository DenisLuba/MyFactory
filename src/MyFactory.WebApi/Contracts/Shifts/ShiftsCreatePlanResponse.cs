namespace MyFactory.WebApi.Contracts.Shifts;

public record ShiftsCreatePlanResponse(
    Guid ShiftPlanId,
    ShiftsStatus Status
);

