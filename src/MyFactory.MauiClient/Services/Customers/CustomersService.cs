using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Customers;

namespace MyFactory.MauiClient.Services.Customers;

public sealed class CustomersService : ICustomersService
{
    private readonly HttpClient _httpClient;

    public CustomersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CustomerListItemResponse>?> GetListAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CustomerListItemResponse>>("api/customers");
    }

    public async Task<CustomerDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<CustomerDetailsResponse>($"api/customers/{id}");
    }

    public async Task<CustomerCardResponse?> GetCardAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<CustomerCardResponse>($"api/customers/{id}/card");
    }

    public async Task<CreateCustomerResponse?> CreateAsync(CreateCustomerRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/customers", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateCustomerResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateCustomerRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/customers/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/customers/{id}");
        response.EnsureSuccessStatusCode();
    }
}
