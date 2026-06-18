namespace MyFactory.WebApi.Contracts.Expences;

public record CreateExpenseRequest(
    Guid ExpenseTypeId,
    DateOnly ExpenseDate,
    decimal Amount,
    string? Description);
