namespace MyFactory.WebApi.Contracts.Finance;

public record RecordOverheadRequest(
    int PeriodMonth,
    int PeriodYear,
    Guid ExpenseTypeId,
    decimal Amount
);

