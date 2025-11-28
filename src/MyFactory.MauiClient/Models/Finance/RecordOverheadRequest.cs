namespace MyFactory.MauiClient.Models.Finance;

public record RecordOverheadRequest(
    int PeriodMonth,
    int PeriodYear,
    Guid ExpenseTypeId,
    decimal Amount);
