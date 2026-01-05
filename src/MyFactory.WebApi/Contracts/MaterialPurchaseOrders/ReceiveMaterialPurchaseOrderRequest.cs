namespace MyFactory.WebApi.Contracts.MaterialPurchaseOrders;

public record ReceiveMaterialPurchaseOrderRequest(Guid WarehouseId, Guid ReceivedByUserId);
