namespace MyFactory.WebApi.Contracts.Finance;

public record OverheadResponse(
    Guid ExpenseTypeId,
    decimal Amount,
    string Period
);

