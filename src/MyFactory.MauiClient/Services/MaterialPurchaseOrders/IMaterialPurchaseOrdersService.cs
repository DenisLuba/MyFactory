using MyFactory.MauiClient.Models.MaterialPurchaseOrders;

namespace MyFactory.MauiClient.Services.MaterialPurchaseOrders;

public interface IMaterialPurchaseOrdersService
{
    Task<CreateMaterialPurchaseOrderResponse?> CreateAsync(CreateMaterialPurchaseOrderRequest request);
    Task AddItemAsync(Guid purchaseOrderId, AddMaterialPurchaseOrderItemRequest request);
    Task ConfirmAsync(Guid purchaseOrderId);
    Task ReceiveAsync(Guid purchaseOrderId, ReceiveMaterialPurchaseOrderRequest request);
    Task<IReadOnlyList<SupplierPurchaseOrderListItemResponse>?> GetBySupplierAsync(Guid supplierId);
    Task<MaterialPurchaseOrderDetailsResponse?> GetDetailsAsync(Guid purchaseOrderId);
    Task UpdateItemAsync(Guid itemId, UpdateMaterialPurchaseOrderItemRequest request);
    Task RemoveItemAsync(Guid itemId);
    Task CancelAsync(Guid purchaseOrderId);
}
