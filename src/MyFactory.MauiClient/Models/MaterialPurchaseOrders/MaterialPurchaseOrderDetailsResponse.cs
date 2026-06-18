namespace MyFactory.MauiClient.Models.MaterialPurchaseOrders;

public sealed record MaterialPurchaseOrderDetailsResponse(
    Guid Id,
    Guid SupplierId,
    decimal PurchaseNumber,
    string SupplierName,
    DateTime OrderDate,
    PurchaseOrderStatus Status,
    IReadOnlyList<MaterialPurchaseOrderItemResponse> Items);
