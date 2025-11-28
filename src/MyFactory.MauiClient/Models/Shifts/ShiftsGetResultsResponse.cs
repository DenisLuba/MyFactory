namespace MyFactory.MauiClient.Models.Shifts;

public record ShiftsGetResultsResponse(
    Guid ShiftPlanId,
    int ActualQty,
    double HoursWorked
);

