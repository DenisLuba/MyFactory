namespace MyFactory.MauiClient.Models.Purchases;

public record PurchasesCreateRequest(
    Guid SupplierId,
    PurchaseItemRequest[] Items);
