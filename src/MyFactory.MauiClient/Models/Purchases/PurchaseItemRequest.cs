namespace MyFactory.MauiClient.Models.Purchases;

public record PurchaseItemRequest(
    Guid MaterialId,
    double Qty);
