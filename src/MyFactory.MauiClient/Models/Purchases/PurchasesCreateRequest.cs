namespace MyFactory.MauiClient.Models.Purchases;

public record PurchasesCreateRequest(
    string DocumentNumber,
    DateTime CreatedAt,
    string WarehouseName,
    Guid? SupplierId,
    string? Comment,
    PurchaseItemRequest[] Items);
