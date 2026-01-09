namespace MyFactory.MauiClient.Models.MaterialPurchaseOrders;

public sealed record MaterialPurchaseOrderItemResponse(
    Guid Id,
    Guid MaterialId,
    string MaterialName,
    string UnitCode,
    decimal Qty,
    decimal UnitPrice);
