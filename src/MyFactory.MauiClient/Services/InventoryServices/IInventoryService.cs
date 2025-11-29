using MyFactory.MauiClient.Models.Inventory;

namespace MyFactory.MauiClient.Services.InventoryServices
{
    public interface IInventoryService
    {
        Task<List<InventoryItemResponse>?> GetAllAsync(string? materialId = null);
        Task<List<InventoryItemResponse>?> GetByWarehouseAsync(string warehouseId);
        Task<CreateInventoryReceiptResponse?> CreateReceiptAsync(CreateInventoryReceiptRequest request);
        Task<AdjustInventoryResponse?> AdjustAsync(AdjustInventoryRequest request);
        Task<TransferInventoryResponse?> TransferAsync(TransferInventoryRequest request);
    }
}