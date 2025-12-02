using System;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Materials;

namespace MyFactory.MauiClient.Services.MaterialsServices;

public class MaterialsService(HttpClient httpClient) : IMaterialsService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<MaterialListResponse>?> ListAsync(string? type = null)
    {
        var query = string.IsNullOrWhiteSpace(type) ? string.Empty : $"?type={type}";
        return await _httpClient.GetFromJsonAsync<List<MaterialListResponse>>($"api/materials{query}");
    }

    public Task<MaterialCardResponse?> GetAsync(string id)
        => _httpClient.GetFromJsonAsync<MaterialCardResponse>($"api/materials/{id}");

    /*public async Task<CreateMaterialResponse?> CreateAsync(CreateMaterialRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/materials", request);
        return await response.Content.ReadFromJsonAsync<CreateMaterialResponse>();
    }

    public async Task<UpdateMaterialResponse?> UpdateAsync(string id, UpdateMaterialRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/materials/{id}", request);
        return await response.Content.ReadFromJsonAsync<UpdateMaterialResponse>();
    }

    public Task<List<MaterialPriceHistoryItem>?> PriceHistoryAsync(string id)
        => _httpClient.GetFromJsonAsync<List<MaterialPriceHistoryItem>>($"api/materials/{id}/price-history");*/

    public async Task<AddMaterialPriceResponse?> AddPriceAsync(string id, AddMaterialPriceRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/materials/{id}/prices", request);
        return await response.Content.ReadFromJsonAsync<AddMaterialPriceResponse>();
    }

    /*public Task<MaterialTypeResponse?> GetMaterialTypeByIdAsync(Guid id)
        => _httpClient.GetFromJsonAsync<MaterialTypeResponse>($"api/materials/type?id={id}");*/
}