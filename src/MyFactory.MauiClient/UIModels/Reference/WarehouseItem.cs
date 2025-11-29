namespace MyFactory.MauiClient.UIModels.Reference;

// Склады
public record WarehouseItem(
    string Id,
    string Name,
    string Type,
    string? Status
);