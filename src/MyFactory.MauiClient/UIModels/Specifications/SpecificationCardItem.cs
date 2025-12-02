namespace MyFactory.MauiClient.UIModels.Specifications;

// Represents the editable fields of a specification card.
public record SpecificationCardItem(
    string Sku,
    string Name,
    decimal PlanPerHour,
    string Description,
    int? ImagesCount
);