namespace MyFactory.MauiClient.Models.MaterialPurchaseOrders;

public sealed record SupplierPurchaseOrderListItemResponse(
    Guid Id,
    DateTime OrderDate,
    PurchaseOrderStatus Status,
    int ItemsCount,
    decimal TotalAmount);

public enum PurchaseOrderStatus
{
    New,
    Confirmed,
    Received,
    Cancelled
}
