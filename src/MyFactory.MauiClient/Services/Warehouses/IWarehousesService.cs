using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.Services.Warehouses;

public interface IWarehousesService
{
    Task<IReadOnlyList<WarehouseListItemResponse>?> GetListAsync(bool includeInactive = false);
    Task<WarehouseInfoResponse?> GetInfoAsync(Guid id);
    Task<IReadOnlyList<WarehouseStockItemResponse>?> GetStockAsync(Guid id);
    Task<CreateWarehouseResponse?> CreateAsync(CreateWarehouseRequest request);
    Task UpdateAsync(Guid id, UpdateWarehouseRequest request);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
    Task RemoveAsync(Guid id);
    Task AddMaterialAsync(Guid warehouseId, AddMaterialToWarehouseRequest request);
    Task AddProductAsync(Guid warehouseId, AddProductToWarehouseRequest request);
    Task RemoveMaterialAsync(Guid warehouseId, Guid materialId);
    Task UpdateMaterialQtyAsync(Guid warehouseId, Guid materialId, UpdateWarehouseMaterialQtyRequest request);
    Task TransferMaterialsAsync(TransferMaterialsRequest request);
    Task TransferProductsAsync(TransferProductsRequest request);
}
