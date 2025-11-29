namespace MyFactory.MauiClient.UIModels.Specifications;

public record SpecificationCardItem(
    string Sku,
    string Name,
    decimal PlanPerHour,
    string Description,
    int ImagesCount
);