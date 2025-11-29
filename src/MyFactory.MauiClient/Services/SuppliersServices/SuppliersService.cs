using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Suppliers;

namespace MyFactory.MauiClient.Services.SuppliersServices;

public class SuppliersService : ISuppliersService
{
    private readonly HttpClient _httpClient;
    public SuppliersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SupplierResponse>?> ListAsync()
        => await _httpClient.GetFromJsonAsync<List<SupplierResponse>>("api/suppliers");

    public async Task<SupplierResponse?> GetAsync(Guid id)
        => await _httpClient.GetFromJsonAsync<SupplierResponse>($"api/suppliers/{id}");

    public async Task<SuppliersCreateUpdateDeleteResponse?> CreateAsync(SuppliersCreateUpdateRequest request)
        => await _httpClient.PostAsJsonAsync("api/suppliers", request)
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SuppliersCreateUpdateDeleteResponse>()).Unwrap();

    public async Task<SuppliersCreateUpdateDeleteResponse?> UpdateAsync(Guid id, SuppliersCreateUpdateRequest request)
        => await _httpClient.PutAsJsonAsync($"api/suppliers/{id}", request)
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SuppliersCreateUpdateDeleteResponse>()).Unwrap();

    public async Task<SuppliersCreateUpdateDeleteResponse?> DeleteAsync(Guid id)
        => await _httpClient.DeleteAsync($"api/suppliers/{id}")
            .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SuppliersCreateUpdateDeleteResponse>()).Unwrap();
}
