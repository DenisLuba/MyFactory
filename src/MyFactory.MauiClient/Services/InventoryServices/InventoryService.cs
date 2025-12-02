using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Inventory;

namespace MyFactory.MauiClient.Services.InventoryServices
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;
        public InventoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<InventoryItemResponse>?> GetAllAsync(string? materialId = null)
            => await _httpClient.GetFromJsonAsync<List<InventoryItemResponse>>($"api/inventory{(materialId != null ? "?materialId=" + materialId : "")}");

        /*public async Task<List<InventoryItemResponse>?> GetByWarehouseAsync(string warehouseId)
            => await _httpClient.GetFromJsonAsync<List<InventoryItemResponse>>($"api/inventory/by-warehouse/{warehouseId}");

        public async Task<CreateInventoryReceiptResponse?> CreateReceiptAsync(CreateInventoryReceiptRequest request)
            => await _httpClient.PostAsJsonAsync("api/inventory/receipt", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<CreateInventoryReceiptResponse>()).Unwrap();

        public async Task<AdjustInventoryResponse?> AdjustAsync(AdjustInventoryRequest request)
            => await _httpClient.PostAsJsonAsync("api/inventory/adjust", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<AdjustInventoryResponse>()).Unwrap();

        public async Task<TransferInventoryResponse?> TransferAsync(TransferInventoryRequest request)
            => await _httpClient.PostAsJsonAsync("api/inventory/transfer", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<TransferInventoryResponse>()).Unwrap();*/
    }
}