using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Workshops;

namespace MyFactory.MauiClient.Services.WorkshopsServices;

public class WorkshopsService(HttpClient httpClient) : IWorkshopsService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyList<WorkshopsListResponse>?> ListAsync()
        => await _httpClient.GetFromJsonAsync<List<WorkshopsListResponse>>("api/workshops");

    public async Task<WorkshopGetResponse?> GetAsync(Guid id)
        => await _httpClient.GetFromJsonAsync<WorkshopGetResponse>($"api/workshops/{id}");

    public async Task<WorkshopCreateResponse?> CreateAsync(WorkshopCreateRequest request)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/workshops", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkshopCreateResponse>();
    }

    public async Task<WorkshopUpdateResponse?> UpdateAsync(Guid id, WorkshopUpdateRequest request)
    {
        using var response = await _httpClient.PutAsJsonAsync($"api/workshops/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkshopUpdateResponse>();
    }
}
