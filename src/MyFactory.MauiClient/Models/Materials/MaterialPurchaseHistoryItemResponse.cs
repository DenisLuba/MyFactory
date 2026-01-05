namespace MyFactory.MauiClient.Models.Materials;

public record MaterialPurchaseHistoryItemResponse(
    Guid SupplierId,
    string SupplierName,
    decimal Qty,
    decimal UnitPrice,
    DateTime PurchaseDate);
