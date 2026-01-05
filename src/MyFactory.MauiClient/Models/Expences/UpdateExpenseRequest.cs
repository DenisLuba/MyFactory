namespace MyFactory.MauiClient.Models.Expences;

public record UpdateExpenseRequest(
    DateOnly ExpenseDate,
    Guid ExpenseTypeId,
    decimal Amount,
    string? Description);
