namespace MyFactory.MauiClient.UIModels.Specifications;

// Карточка спецификации
public record SpecificationCardItem(
    string Sku,
    string Name,
    decimal PlanPerHour,
    string Description,
    int? ImagesCount
);