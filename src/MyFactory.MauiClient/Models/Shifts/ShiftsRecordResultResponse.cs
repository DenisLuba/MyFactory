namespace MyFactory.MauiClient.Models.Shifts;

public record ShiftsRecordResultResponse(
    Guid ShiftPlanId,
    ShiftsStatus Status
);

