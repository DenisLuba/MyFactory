namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationOperationItemResponse(
    Guid Id,
    string Operation,
    double Minutes,
    decimal Cost
);
