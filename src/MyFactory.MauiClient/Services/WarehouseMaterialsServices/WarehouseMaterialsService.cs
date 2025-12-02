using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.WarehouseMaterials;

namespace MyFactory.MauiClient.Services.WarehouseMaterialsServices;

public class WarehouseMaterialsService(HttpClient httpClient) : IWarehouseMaterialsService
{
    private readonly HttpClient _httpClient = httpClient;

    public Task<List<MaterialReceiptListResponse>?> ListReceiptsAsync()
        => _httpClient.GetFromJsonAsync<List<MaterialReceiptListResponse>>("api/material-receipts");

    public Task<MaterialReceiptCardResponse?> GetReceiptAsync(Guid id)
        => _httpClient.GetFromJsonAsync<MaterialReceiptCardResponse>($"api/material-receipts/{id}");

    public async Task<MaterialReceiptUpsertResponse?> CreateReceiptAsync(MaterialReceiptUpsertRequest request)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/material-receipts", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MaterialReceiptUpsertResponse>();
    }

    public async Task<MaterialReceiptUpsertResponse?> UpdateReceiptAsync(Guid id, MaterialReceiptUpsertRequest request)
    {
        using var response = await _httpClient.PutAsJsonAsync($"api/material-receipts/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MaterialReceiptUpsertResponse>();
    }

    public async Task<MaterialReceiptPostResponse?> PostReceiptAsync(Guid id)
    {
        using var response = await _httpClient.PostAsync($"api/material-receipts/{id}/post", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MaterialReceiptPostResponse>();
    }

    public Task<List<MaterialReceiptLineResponse>?> GetLinesAsync(Guid id)
        => _httpClient.GetFromJsonAsync<List<MaterialReceiptLineResponse>>($"api/material-receipts/{id}/lines");

    public async Task<MaterialReceiptLineUpsertResponse?> AddLineAsync(Guid id, MaterialReceiptLineUpsertRequest request)
    {
        using var response = await _httpClient.PostAsJsonAsync($"api/material-receipts/{id}/lines", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MaterialReceiptLineUpsertResponse>();
    }

    public async Task<MaterialReceiptLineUpsertResponse?> UpdateLineAsync(Guid id, Guid lineId, MaterialReceiptLineUpsertRequest request)
    {
        using var response = await _httpClient.PutAsJsonAsync($"api/material-receipts/{id}/lines/{lineId}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MaterialReceiptLineUpsertResponse>();
    }

    public async Task DeleteLineAsync(Guid id, Guid lineId)
    {
        using var response = await _httpClient.DeleteAsync($"api/material-receipts/{id}/lines/{lineId}");
        response.EnsureSuccessStatusCode();
    }
}
