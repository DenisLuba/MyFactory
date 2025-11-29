namespace MyFactory.MauiClient.UIModels.Reference;

// Цеха / Участки
public record WorkshopItem(
    string Id,
    string Name,
    string Type,
    string? Status
);