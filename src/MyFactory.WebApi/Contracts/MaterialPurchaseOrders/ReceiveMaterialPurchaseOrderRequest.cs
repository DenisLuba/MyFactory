namespace MyFactory.WebApi.Contracts.MaterialPurchaseOrders;

public record ReceiveMaterialPurchaseOrderRequest(
    Guid ReceivedByUserId,
    IReadOnlyList<ReceiveMaterialPurchaseOrderItemRequest> Items);

public record ReceiveMaterialPurchaseOrderItemRequest(
    Guid ItemId,
    IReadOnlyList<ReceiveMaterialPurchaseOrderAllocationRequest> Allocations);

public record ReceiveMaterialPurchaseOrderAllocationRequest(
    Guid WarehouseId,
    decimal Qty);
