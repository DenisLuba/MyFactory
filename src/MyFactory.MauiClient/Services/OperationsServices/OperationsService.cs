using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Operations;

namespace MyFactory.MauiClient.Services.OperationsServices;

public class OperationsService(HttpClient httpClient) : IOperationsService
{
    private readonly HttpClient _httpClient = httpClient;

    public Task<IReadOnlyList<OperationListResponse>?> GetOperationsAsync()
        => _httpClient.GetFromJsonAsync<IReadOnlyList<OperationListResponse>>("api/operations");

    public Task<OperationCardResponse?> GetOperationAsync(Guid id)
        => _httpClient.GetFromJsonAsync<OperationCardResponse>($"api/operations/{id}");

    public async Task<OperationUpdateResponse?> UpdateOperationAsync(Guid id, OperationUpdateRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/operations/{id}", request);
        return await response.Content.ReadFromJsonAsync<OperationUpdateResponse>();
    }
}
