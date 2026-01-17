using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Warehouses;

public sealed class WarehousesService : IWarehousesService
{
    private readonly HttpClient _httpClient;

    public WarehousesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<WarehouseListItemResponse>?> GetListAsync(bool includeInactive = false)
    {
        var query = includeInactive ? "?includeInactive=true" : string.Empty;
        return await _httpClient.GetFromJsonAsync<List<WarehouseListItemResponse>>($"api/warehouses{query}");
    }

    public async Task<WarehouseInfoResponse?> GetInfoAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<WarehouseInfoResponse>($"api/warehouses/{id}");
    }

    public async Task<IReadOnlyList<WarehouseStockItemResponse>?> GetStockAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<List<WarehouseStockItemResponse>>($"api/warehouses/{id}/stock");
    }

    public async Task<CreateWarehouseResponse?> CreateAsync(CreateWarehouseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/warehouses", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateWarehouseResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateWarehouseRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/warehouses/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task ActivateAsync(Guid id)
    {
        var response = await _httpClient.PutAsync($"api/warehouses/{id}/activate", null);
        await response.EnsureSuccessWithProblemAsync();

    }

    public async Task DeactivateAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/warehouses/{id}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/warehouses/{id}/remove");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task AddMaterialAsync(Guid warehouseId, AddMaterialToWarehouseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/warehouses/{warehouseId}/materials", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task RemoveMaterialAsync(Guid warehouseId, Guid materialId)
    {
        var response = await _httpClient.DeleteAsync($"api/warehouses/{warehouseId}/materials/{materialId}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task UpdateMaterialQtyAsync(Guid warehouseId, Guid materialId, UpdateWarehouseMaterialQtyRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/warehouses/{warehouseId}/materials/{materialId}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task TransferMaterialsAsync(TransferMaterialsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/warehouses/materials/transfer", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task TransferProductsAsync(TransferProductsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/warehouses/products/transfer", request);
        await response.EnsureSuccessWithProblemAsync();
    }
}
