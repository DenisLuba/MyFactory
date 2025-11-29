using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Production;

namespace MyFactory.MauiClient.Services.ProductionServices
{
    public class ProductionService(HttpClient httpClient) : IProductionService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ProductionCreateOrderResponse?> CreateOrderAsync(ProductionCreateOrderRequest request)
            => await _httpClient.PostAsJsonAsync("api/production/orders", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ProductionCreateOrderResponse>()).Unwrap();

        public async Task<ProductionGetOrderResponse?> GetOrderAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<ProductionGetOrderResponse>($"api/production/orders/{id}");

        public async Task<ProductionGetOrderStatusResponse?> GetOrderStatusAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<ProductionGetOrderStatusResponse>($"api/production/orders/{id}/status");

        public async Task<ProductionAllocateResponse?> AllocateAsync(Guid id, ProductionAllocateRequest request)
            => await _httpClient.PostAsJsonAsync($"api/production/orders/{id}/allocate", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ProductionAllocateResponse>()).Unwrap();

        public async Task<ProductionRecordStageResponse?> RecordStageAsync(Guid id, ProductionRecordStageRequest request)
            => await _httpClient.PostAsJsonAsync($"api/production/orders/{id}/stages", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ProductionRecordStageResponse>()).Unwrap();

        public async Task<ProductionAssignWorkerResponse?> AssignWorkerAsync(Guid id, ProductionAssignWorkerRequest request)
            => await _httpClient.PostAsJsonAsync($"api/production/orders/{id}/assign", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ProductionAssignWorkerResponse>()).Unwrap();
    }
}