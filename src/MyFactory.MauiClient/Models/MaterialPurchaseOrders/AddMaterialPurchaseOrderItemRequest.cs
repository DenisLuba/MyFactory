namespace MyFactory.MauiClient.Models.MaterialPurchaseOrders;

public record AddMaterialPurchaseOrderItemRequest(Guid MaterialId, decimal Qty, decimal UnitPrice);
