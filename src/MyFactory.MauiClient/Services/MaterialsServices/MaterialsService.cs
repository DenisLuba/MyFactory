using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Materials;

namespace MyFactory.MauiClient.Services.MaterialsServices
{
    public class MaterialsService(HttpClient httpClient) : IMaterialsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<MaterialResponse>?> ListAsync(string? type = null)
            => await _httpClient.GetFromJsonAsync<List<MaterialResponse>>($"api/materials{(type != null ? "?type=" + type : "")}");

        public async Task<MaterialResponse?> GetAsync(string id)
            => await _httpClient.GetFromJsonAsync<MaterialResponse>($"api/materials/{id}");

        public async Task<CreateMaterialResponse?> CreateAsync(CreateMaterialRequest request)
            => await _httpClient.PostAsJsonAsync("api/materials", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<CreateMaterialResponse>()).Unwrap();

        public async Task<UpdateMaterialResponse?> UpdateAsync(string id, UpdateMaterialRequest request)
            => await _httpClient.PutAsJsonAsync($"api/materials/{id}", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<UpdateMaterialResponse>()).Unwrap();

        public async Task<List<MaterialPriceHistoryResponse>?> PriceHistoryAsync(string id)
            => await _httpClient.GetFromJsonAsync<List<MaterialPriceHistoryResponse>>($"api/materials/{id}/price-history");

        public async Task<AddMaterialPriceResponse?> AddPriceAsync(string id, AddMaterialPriceRequest request)
            => await _httpClient.PostAsJsonAsync($"api/materials/{id}/prices", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<AddMaterialPriceResponse>()).Unwrap();

        public async Task<MaterialTypeResponse?> GetMaterialTypeByIdAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<MaterialTypeResponse>($"api/materials/type?id={id}");
    }
}