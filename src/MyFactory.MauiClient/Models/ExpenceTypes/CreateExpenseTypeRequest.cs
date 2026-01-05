namespace MyFactory.MauiClient.Models.ExpenceTypes;

public record CreateExpenseTypeRequest(
    string Name,
    string? Description);
