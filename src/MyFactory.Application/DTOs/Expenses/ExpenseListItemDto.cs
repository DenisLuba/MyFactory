namespace MyFactory.Application.DTOs.Expenses;

public sealed record ExpenseListItemDto(
    Guid Id,
    DateOnly ExpenseDate,
    string ExpenseTypeName,
    decimal Amount,
    string? Description,
    string CreatedBy
);
