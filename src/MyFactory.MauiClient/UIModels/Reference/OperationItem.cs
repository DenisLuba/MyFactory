namespace MyFactory.MauiClient.UIModels.Reference;

// Операции
public record OperationItem(
    string Id,
    string Code,
    string Name,
    string OperationType,
    int TimeMinutes,
    decimal Cost
);