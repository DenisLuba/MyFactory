namespace MyFactory.MauiClient.Models.MaterialPurchaseOrders;

public record CreateMaterialPurchaseOrderRequest(Guid SupplierId, DateTime OrderDate);
