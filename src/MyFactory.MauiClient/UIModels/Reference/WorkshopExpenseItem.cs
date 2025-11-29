namespace MyFactory.MauiClient.UIModels.Reference;

// Цеховые расходы (история)
public record WorkshopExpenseItem(
    string Id,
    string Workshop,
    string? Product,
    decimal AmountPerUnit,
    string Period
);