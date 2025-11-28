namespace MyFactory.MauiClient.Models.Purchases;

public record PurchasesConvertToOrderResponse(
    Guid PurchaseId,
    PurchasesStatus Status);
