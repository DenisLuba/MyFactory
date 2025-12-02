using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Production.MaterialTransfers;

namespace MyFactory.MauiClient.Services.ProductionServices;

public class MaterialTransfersService(HttpClient httpClient) : IMaterialTransfersService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyList<MaterialTransferListResponse>?> GetTransfersAsync(DateTime? dateFilter = null, string? warehouse = null, string? productionOrder = null)
    {
        var query = new List<string>();
        if (dateFilter.HasValue)
        {
            query.Add($"date={dateFilter.Value:yyyy-MM-dd}");
        }

        if (!string.IsNullOrWhiteSpace(warehouse))
        {
            query.Add($"warehouse={Uri.EscapeDataString(warehouse)}");
        }

        if (!string.IsNullOrWhiteSpace(productionOrder))
        {
            query.Add($"productionOrder={Uri.EscapeDataString(productionOrder)}");
        }

        var queryString = query.Count > 0 ? "?" + string.Join("&", query) : string.Empty;
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<MaterialTransferListResponse>>($"api/material-transfers{queryString}");
    }

    public async Task<MaterialTransferCardResponse?> GetByIdAsync(Guid transferId)
        => await _httpClient.GetFromJsonAsync<MaterialTransferCardResponse>($"api/material-transfers/{transferId}");

    /*public async Task<MaterialTransferCreateResponse?> CreateAsync(MaterialTransferCreateRequest request)
        => await _httpClient.PostAsJsonAsync("api/material-transfers", request)
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<MaterialTransferCreateResponse>()).Unwrap();

    public async Task<MaterialTransferUpdateResponse?> UpdateAsync(Guid transferId, MaterialTransferUpdateRequest request)
        => await _httpClient.PutAsJsonAsync($"api/material-transfers/{transferId}", request)
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<MaterialTransferUpdateResponse>()).Unwrap();

    public async Task<MaterialTransferDeleteResponse?> DeleteAsync(Guid transferId)
        => await _httpClient.DeleteAsync($"api/material-transfers/{transferId}")
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<MaterialTransferDeleteResponse>()).Unwrap();*/

    public async Task<MaterialTransferSubmitResponse?> SubmitAsync(Guid transferId)
        => await _httpClient.PostAsync($"api/material-transfers/{transferId}/submit", null)
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<MaterialTransferSubmitResponse>()).Unwrap();
}
