namespace MyFactory.MauiClient.UIModels.Specifications;

public record SpecificationBomItem(
    string Material,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal TotalCost
);