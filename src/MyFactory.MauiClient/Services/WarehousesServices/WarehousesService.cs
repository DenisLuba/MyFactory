using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.Services.WarehousesServices;

public class WarehousesService(HttpClient httpClient) : IWarehousesService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<WarehousesListResponse>?> ListAsync()
        => await _httpClient.GetFromJsonAsync<List<WarehousesListResponse>>("api/warehouses");

    public async Task<WarehousesGetResponse?> GetAsync(Guid id)
        => await _httpClient.GetFromJsonAsync<WarehousesGetResponse>($"api/warehouses/{id}");

    public async Task<WarehousesCreateResponse?> CreateAsync(WarehousesCreateRequest request)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/warehouses", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WarehousesCreateResponse>();
    }

    public async Task<WarehousesUpdateResponse?> UpdateAsync(Guid id, WarehousesUpdateRequest request)
    {
        using var response = await _httpClient.PutAsJsonAsync($"api/warehouses/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WarehousesUpdateResponse>();
    }

    public async Task DeleteAsync(Guid id)
    {
        using var response = await _httpClient.DeleteAsync($"api/warehouses/{id}");
        response.EnsureSuccessStatusCode();
    }
}