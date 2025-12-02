namespace MyFactory.MauiClient.UIModels.Specifications;

public record SpecificationsListItem(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    string Status,
    int ImagesCount
);
