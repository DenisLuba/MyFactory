using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.Services.WarehousesServices
{
    public class WarehousesService(HttpClient httpClient) : IWarehousesService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<WarehousesGetResponse>?> ListAsync()
            => await _httpClient.GetFromJsonAsync<List<WarehousesGetResponse>>("api/warehouses");

        public async Task<WarehousesGetResponse?> GetAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<WarehousesGetResponse>($"api/warehouses/{id}");

        public async Task<WarehousesCreateResponse?> CreateAsync(WarehousesCreateRequest request)
            => await _httpClient.PostAsJsonAsync("api/warehouses", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<WarehousesCreateResponse>()).Unwrap();

        public async Task<WarehousesUpdateResponse?> UpdateAsync(Guid id, WarehousesUpdateRequest request)
            => await _httpClient.PutAsJsonAsync($"api/warehouses/{id}", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<WarehousesUpdateResponse>()).Unwrap();
    }
}