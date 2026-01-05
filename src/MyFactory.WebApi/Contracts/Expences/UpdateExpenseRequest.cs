namespace MyFactory.WebApi.Contracts.Expences;

public record UpdateExpenseRequest(
    DateOnly ExpenseDate,
    Guid ExpenseTypeId,
    decimal Amount,
    string? Description);
