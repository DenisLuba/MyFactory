namespace MyFactory.MauiClient.UIModels.Reference;

public record MaterialItem(
    string Code,
    string Name,
    string Type,
    string Unit,
    string Status,
    decimal LastPrice
);