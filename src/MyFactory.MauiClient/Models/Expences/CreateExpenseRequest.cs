namespace MyFactory.MauiClient.Models.Expences;

public record CreateExpenseRequest(
    Guid ExpenseTypeId,
    DateOnly ExpenseDate,
    decimal Amount,
    string? Description);
