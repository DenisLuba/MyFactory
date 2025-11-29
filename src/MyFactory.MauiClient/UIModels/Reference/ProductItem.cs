namespace MyFactory.MauiClient.UIModels.Reference;

// Изделия (справочник)
public record ProductItem(
    string Id,
    string Article,
    string Name,
    decimal PlanPerHour,
    string Status,
    int ImagesCount
);