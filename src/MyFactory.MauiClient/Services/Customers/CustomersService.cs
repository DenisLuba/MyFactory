using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Services.Common;
using System.Net.Http.Json;

namespace MyFactory.MauiClient.Services.Customers;

public sealed class CustomersService(HttpClient httpClient) : ICustomersService
{
    public async Task<ListResponse<CustomerListItemResponse>?> GetListAsync(
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

        var path = "api/customers" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        return await httpClient.GetFromJsonAsync<ListResponse<CustomerListItemResponse>?>(path);
    }

    public async Task<CustomerDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await httpClient.GetFromJsonAsync<CustomerDetailsResponse>($"api/customers/{id}");
    }

    public async Task<CustomerCardResponse?> GetCardAsync(Guid id)
    {
        return await httpClient.GetFromJsonAsync<CustomerCardResponse>($"api/customers/{id}/card");
    }

    public async Task<CreateCustomerResponse?> CreateAsync(CreateCustomerRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/customers", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateCustomerResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateCustomerRequest request)
    {
        var response = await httpClient.PutAsJsonAsync($"api/customers/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"api/customers/{id}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task ActivateAsync(Guid id)
    {
        var response = await httpClient.PutAsJsonAsync($"api/customers/{id}/activate", true);
        await response.EnsureSuccessWithProblemAsync();
    }
}
