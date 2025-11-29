namespace MyFactory.MauiClient.UIModels.Reference;

public record OperationItem(
    string Id,
    string Code,
    string Name,
    string OperationType,
    int TimeMinutes,
    decimal Cost
);