namespace MyFactory.MauiClient.Models.ExpenceTypes;

public record ExpenseTypeResponse(
    Guid Id,
    string Name,
    string? Description);
