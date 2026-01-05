namespace MyFactory.MauiClient.Models.Advances;

public record CreateCashAdvanceExpenseRequest(
    DateOnly ExpenseDate,
    decimal Amount,
    string? Description);
