namespace MyFactory.MauiClient.UIModels.Specifications;

// Aggregated cost values for a specification.
public record SpecificationCostItem(
    decimal MaterialsCost,
    decimal OperationsCost,
    decimal WorkshopExpenses,
    decimal TotalCost
);