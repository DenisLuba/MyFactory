namespace MyFactory.MauiClient.Models.Suppliers;

public sealed record SupplierPurchaseHistoryResponse(
    string MaterialType,
    string MaterialName,
    decimal Qty,
    decimal UnitPrice,
    DateTime Date);
