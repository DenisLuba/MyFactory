namespace MyFactory.MauiClient.UIModels.Specifications;

// BOM - Материалы спецификации
public record SpecificationBomItem(
    string Material,
    decimal Quantity,
    string Unit,
    decimal AvgPrice,
    decimal TotalCost
);