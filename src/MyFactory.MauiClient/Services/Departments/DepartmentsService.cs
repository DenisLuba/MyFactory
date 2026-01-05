using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Departments;

namespace MyFactory.MauiClient.Services.Departments;

public sealed class DepartmentsService : IDepartmentsService
{
    private readonly HttpClient _httpClient;

    public DepartmentsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<DepartmentListItemResponse>?> GetListAsync(bool includeInactive = false)
    {
        var url = includeInactive ? "api/departments?includeInactive=true" : "api/departments";
        return await _httpClient.GetFromJsonAsync<List<DepartmentListItemResponse>>(url);
    }

    public async Task<DepartmentDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<DepartmentDetailsResponse>($"api/departments/{id}");
    }

    public async Task<CreateDepartmentResponse?> CreateAsync(CreateDepartmentRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/departments", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateDepartmentResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateDepartmentRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/departments/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task ActivateAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/departments/{id}/activate", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/departments/{id}/deactivate", null);
        response.EnsureSuccessStatusCode();
    }
}
