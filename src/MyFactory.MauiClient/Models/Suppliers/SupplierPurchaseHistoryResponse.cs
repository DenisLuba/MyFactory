using MyFactory.MauiClient.Models.MaterialPurchaseOrders;

namespace MyFactory.MauiClient.Models.Suppliers;

public sealed record SupplierPurchaseHistoryResponse(
    Guid OrderId,
    string MaterialType,
    string MaterialName,
    decimal Qty,
    decimal UnitPrice,
    DateTime Date,
    PurchaseOrderStatus Status);
