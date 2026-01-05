using System.Net.Http.Json;
using MyFactory.MauiClient.Models.ExpenceTypes;

namespace MyFactory.MauiClient.Services.ExpenceTypes;

public sealed class ExpenceTypesService : IExpenceTypesService
{
    private readonly HttpClient _httpClient;

    public ExpenceTypesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ExpenseTypeResponse>?> GetListAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ExpenseTypeResponse>>("api/expencetypes");
    }

    public async Task<ExpenseTypeResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<ExpenseTypeResponse>($"api/expencetypes/{id}");
    }

    public async Task<CreateExpenseTypeResponse?> CreateAsync(CreateExpenseTypeRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/expencetypes", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateExpenseTypeResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateExpenseTypeRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/expencetypes/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/expencetypes/{id}");
        response.EnsureSuccessStatusCode();
    }
}
