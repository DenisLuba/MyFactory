using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Purchases;

namespace MyFactory.MauiClient.Services.PurchasesServices
{
    public class PurchasesService(HttpClient httpClient) : IPurchasesService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<PurchasesCreateResponse?> CreatePurchaseAsync(PurchasesCreateRequest request)
            => await _httpClient.PostAsJsonAsync("api/purchases/purchase-requests", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<PurchasesCreateResponse>()).Unwrap();

        public async Task<List<PurchasesResponse>?> PurchasesListAsync()
            => await _httpClient.GetFromJsonAsync<List<PurchasesResponse>>("api/purchases/requests");

        public async Task<PurchasesConvertToOrderResponse?> ConvertToOrderAsync(string id)
            => await _httpClient.PostAsync($"api/purchases/requests/{id}/convert-to-order", null)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<PurchasesConvertToOrderResponse>()).Unwrap();
    }
}