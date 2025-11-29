namespace MyFactory.MauiClient.UIModels.Specifications;

public record SpecificationListItem(
    string Id,
    string Sku,
    string Name,
    decimal PlanPerHour,
    string Status,
    int Version
);