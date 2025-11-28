namespace MyFactory.MauiClient.Models.Purchases;

public record PurchasesCreateResponse(
    Guid PurchaseId,
    PurchasesStatus Status);
