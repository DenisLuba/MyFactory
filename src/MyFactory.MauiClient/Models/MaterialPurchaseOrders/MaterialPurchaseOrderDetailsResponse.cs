namespace MyFactory.MauiClient.Models.MaterialPurchaseOrders;

public sealed record MaterialPurchaseOrderDetailsResponse(
    Guid Id,
    Guid SupplierId,
    string SupplierName,
    DateTime OrderDate,
    PurchaseOrderStatus Status,
    IReadOnlyList<MaterialPurchaseOrderItemResponse> Items);
