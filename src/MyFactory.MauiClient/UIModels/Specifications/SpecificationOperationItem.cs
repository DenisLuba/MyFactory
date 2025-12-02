namespace MyFactory.MauiClient.UIModels.Specifications;

// Represents a single production operation linked to the specification.
public record SpecificationOperationItem(
    string Operation,
    double Minutes,
    decimal Cost
);