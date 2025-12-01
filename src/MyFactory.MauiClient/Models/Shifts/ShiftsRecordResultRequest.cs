namespace MyFactory.MauiClient.Models.Shifts;

public record ShiftsRecordResultRequest(
    Guid ShiftPlanId,
    int ActualQty,
    double HoursWorked,
    bool Bonus
);

