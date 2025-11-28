namespace MyFactory.MauiClient.Models.Purchases;

public record PurchasesResponse(
    Guid PurchaseId,
    DateTime CreatedAt,
    PurchasesStatus Status,
    PurchaseResponseItem[] Items);
