using System;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Purchases;

namespace MyFactory.MauiClient.Services.PurchasesServices
{
    public class PurchasesService(HttpClient httpClient) : IPurchasesService
    {
        private readonly HttpClient _httpClient = httpClient;

        public Task<List<PurchasesResponse>?> PurchasesListAsync()
            => _httpClient.GetFromJsonAsync<List<PurchasesResponse>>("api/purchases/requests");

        public Task<PurchaseRequestDetailResponse?> GetPurchaseRequestAsync(Guid id)
            => _httpClient.GetFromJsonAsync<PurchaseRequestDetailResponse>($"api/purchases/requests/{id}");

        public async Task<PurchasesCreateResponse?> CreatePurchaseAsync(PurchasesCreateRequest request)
        {
            using var response = await _httpClient.PostAsJsonAsync("api/purchases/requests", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PurchasesCreateResponse>();
        }

        public async Task<PurchasesCreateResponse?> UpdatePurchaseAsync(Guid id, PurchasesCreateRequest request)
        {
            using var response = await _httpClient.PutAsJsonAsync($"api/purchases/requests/{id}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PurchasesCreateResponse>();
        }

        public async Task DeletePurchaseAsync(Guid id)
        {
            using var response = await _httpClient.DeleteAsync($"api/purchases/requests/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<PurchasesConvertToOrderResponse?> ConvertToOrderAsync(Guid id)
        {
            using var response = await _httpClient.PostAsync($"api/purchases/requests/{id}/convert-to-order", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PurchasesConvertToOrderResponse>();
        }
    }
}