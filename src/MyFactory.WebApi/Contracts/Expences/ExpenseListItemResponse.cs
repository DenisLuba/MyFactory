namespace MyFactory.WebApi.Contracts.Expences;

public record ExpenseListItemResponse(
    Guid Id,
    DateOnly ExpenseDate,
    string ExpenseTypeName,
    decimal Amount,
    string? Description,
    string CreatedBy);
