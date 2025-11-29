namespace MyFactory.MauiClient.UIModels.Specifications;

// Операции (спецификация)
public record SpecificationOperationItem(
    string Operation,
    int TimeMinutes,
    decimal Cost
);