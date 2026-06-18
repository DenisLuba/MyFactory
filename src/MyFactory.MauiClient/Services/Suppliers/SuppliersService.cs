using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.Common;
using System.Net.Http.Json;

namespace MyFactory.MauiClient.Services.Suppliers;

public sealed class SuppliersService : ISuppliersService
{
    private readonly HttpClient _httpClient;

    public SuppliersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ListResponse<SupplierListItemResponse>?> GetListAsync(
        string? searchName =null,
        string? searchType = null,
        string? sortBy = null,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        bool? isActive = null,
        Guid? fromId = null)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(searchName)) 
            query.Add($"searchName={Uri.EscapeDataString(searchName)}");
        if (!string.IsNullOrWhiteSpace(sortBy)) query.Add($"sortBy={Uri.EscapeDataString(sortBy)}");
        if (sortDesc) query.Add("sortDesc=true");
        query.Add($"skip={skip}");
        query.Add($"take={take}");
        if (isActive.HasValue)
            query.Add($"isActive={isActive.Value.ToString().ToLowerInvariant()}");

        var path = "api/suppliers" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        return await _httpClient.GetFromJsonAsync<ListResponse<SupplierListItemResponse>>(path);
    }

    public async Task<SupplierDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<SupplierDetailsResponse>($"api/suppliers/{id}");
    }

    public async Task<CreateSupplierResponse?> CreateAsync(CreateSupplierRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/suppliers", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateSupplierResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateSupplierRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/suppliers/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/suppliers/{id}");
        await response.EnsureSuccessWithProblemAsync();
    }
}
