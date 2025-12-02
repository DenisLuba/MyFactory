using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.WorkshopExpenses;

namespace MyFactory.MauiClient.Services.WorkshopExpensesServices;

public class WorkshopExpensesService(HttpClient httpClient) : IWorkshopExpensesService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyList<WorkshopExpenseListResponse>?> ListAsync(Guid? workshopId = null)
    {
        var url = "api/workshops/expenses";
        if (workshopId.HasValue)
        {
            url += $"?workshopId={workshopId.Value}";
        }

        return await _httpClient.GetFromJsonAsync<List<WorkshopExpenseListResponse>>(url);
    }

    public async Task<WorkshopExpenseGetResponse?> GetAsync(Guid id)
        => await _httpClient.GetFromJsonAsync<WorkshopExpenseGetResponse>($"api/workshops/expenses/{id}");

    public async Task<WorkshopExpenseCreateResponse?> CreateAsync(WorkshopExpenseCreateRequest request)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/workshops/expenses", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkshopExpenseCreateResponse>();
    }

    public async Task<WorkshopExpenseUpdateResponse?> UpdateAsync(Guid id, WorkshopExpenseUpdateRequest request)
    {
        using var response = await _httpClient.PutAsJsonAsync($"api/workshops/expenses/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkshopExpenseUpdateResponse>();
    }
}
