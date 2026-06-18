namespace MyFactory.WebApi.Contracts.MaterialPurchaseOrders;

public record AddMaterialPurchaseOrderItemRequest(Guid MaterialId, decimal Qty, decimal UnitPrice);
