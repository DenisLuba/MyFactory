namespace MyFactory.MauiClient.UIModels.Reference;

// Материалы
public record MaterialItem(
    string Code,
    string Name,
    string Type,
    string Unit,
    string Status,
    decimal LastPrice
);