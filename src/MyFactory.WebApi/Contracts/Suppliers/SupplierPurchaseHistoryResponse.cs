namespace MyFactory.WebApi.Contracts.Suppliers;

public sealed record SupplierPurchaseHistoryResponse(
    string MaterialType,
    string MaterialName,
    decimal Qty,
    decimal UnitPrice,
    DateTime Date);
