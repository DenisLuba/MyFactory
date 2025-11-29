namespace MyFactory.MauiClient.UIModels.Reference;

public record WorkshopExpenseItem(
    string Id,
    string Workshop,
    decimal AmountPerUnit,
    string Period
);