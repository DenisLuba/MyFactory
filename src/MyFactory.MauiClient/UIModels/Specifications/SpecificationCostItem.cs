namespace MyFactory.MauiClient.UIModels.Specifications;

public record SpecificationCostItem(
    string Article,
    decimal MaterialsCost,
    decimal OperationsCost,
    decimal WorkshopExpenses,
    decimal TotalCost
);