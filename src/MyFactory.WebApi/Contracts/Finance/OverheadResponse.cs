namespace MyFactory.WebApi.Contracts.Finance;

public record OverheadResponse(
    string ExpenseType,
    decimal Amount,
    string Period
);

