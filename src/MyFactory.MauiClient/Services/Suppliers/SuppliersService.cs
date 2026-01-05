using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Suppliers;

namespace MyFactory.MauiClient.Services.Suppliers;

public sealed class SuppliersService : ISuppliersService
{
    private readonly HttpClient _httpClient;

    public SuppliersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<SupplierListItemResponse>?> GetListAsync(string? search = null)
    {
        var query = string.IsNullOrWhiteSpace(search)
            ? string.Empty
            : $"?search={Uri.EscapeDataString(search)}";

        return await _httpClient.GetFromJsonAsync<List<SupplierListItemResponse>>($"api/suppliers{query}");
    }

    public async Task<SupplierDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<SupplierDetailsResponse>($"api/suppliers/{id}");
    }

    public async Task<CreateSupplierResponse?> CreateAsync(CreateSupplierRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/suppliers", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateSupplierResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateSupplierRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/suppliers/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/suppliers/{id}");
        response.EnsureSuccessStatusCode();
    }
}
