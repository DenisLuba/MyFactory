namespace MyFactory.WebApi.Contracts.MaterialPurchaseOrders;

public record CreateMaterialPurchaseOrderRequest(Guid SupplierId, DateTime OrderDate);
