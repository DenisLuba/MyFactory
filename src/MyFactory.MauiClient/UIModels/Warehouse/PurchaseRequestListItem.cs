namespace MyFactory.MauiClient.UIModels.Warehouse;

public record PurchaseRequestListItem(
    Guid PurchaseId,
    string DocumentNumber,
    DateTime CreatedAt,
    string ItemsSummary,
    decimal TotalAmount,
    string StatusDisplay
);
