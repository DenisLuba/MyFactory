namespace MyFactory.MauiClient.UIModels.Specifications;

public record SpecificationOperationItem(
    string Operation,
    int TimeMinutes,
    decimal Cost
);