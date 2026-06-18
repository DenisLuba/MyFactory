namespace MyFactory.WebApi.Contracts.Advances;

public record CreateCashAdvanceExpenseRequest(
    DateOnly ExpenseDate,
    decimal Amount,
    string? Description);
