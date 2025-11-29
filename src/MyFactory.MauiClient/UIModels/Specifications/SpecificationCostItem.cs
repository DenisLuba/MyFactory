namespace MyFactory.MauiClient.UIModels.Specifications;

// Карточка себестоимости (итог)
public record SpecificationCostItem(
    decimal MaterialsCost,
    decimal OperationsCost,
    decimal WorkshopExpenses,
    decimal TotalCost
);