namespace MyFactory.MauiClient.Models.Shifts;

public record ShiftsCreatePlanResponse(
    Guid ShiftPlanId,
    ShiftsStatus Status
);

