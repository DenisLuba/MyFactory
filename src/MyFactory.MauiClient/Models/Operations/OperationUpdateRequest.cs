namespace MyFactory.MauiClient.Models.Operations;

public record OperationUpdateRequest(
    string Code,
    string Name,
    string OperationType,
    double Minutes,
    decimal Cost
);
