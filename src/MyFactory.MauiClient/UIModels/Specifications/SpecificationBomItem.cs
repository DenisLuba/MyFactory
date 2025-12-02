namespace MyFactory.MauiClient.UIModels.Specifications;

// Represents a single BOM line in the specification.
public record SpecificationBomItem(
    string Material,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal TotalCost
);