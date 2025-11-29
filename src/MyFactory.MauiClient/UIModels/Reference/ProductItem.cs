namespace MyFactory.MauiClient.UIModels.Reference;

public record ProductItem(
    string Id,
    string Article,
    string Name,
    decimal PlanPerHour,
    string Status,
    int ImagesCount
);