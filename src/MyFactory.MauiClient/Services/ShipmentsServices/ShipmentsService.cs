using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Shipments;

namespace MyFactory.MauiClient.Services.ShipmentsServices
{
    public class ShipmentsService(HttpClient httpClient) : IShipmentsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ShipmentsCreateResponse?> CreateShipmentAsync(ShipmentsCreateRequest request)
            => await _httpClient.PostAsJsonAsync("api/shipments", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ShipmentsCreateResponse>()).Unwrap();

        public async Task<ShipmentsGetResponse?> GetAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<ShipmentsGetResponse>($"api/shipments/{id}");

        public async Task<ShipmentsConfirmPaymentResponse?> ConfirmPaymentAsync(Guid id)
            => await _httpClient.PostAsync($"api/shipments/{id}/confirm-payment", null)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ShipmentsConfirmPaymentResponse>()).Unwrap();
    }
}