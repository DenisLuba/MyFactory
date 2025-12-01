using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Employees;

namespace MyFactory.MauiClient.Services.EmployeesServices;

public class EmployeesService(HttpClient httpClient) : IEmployeesService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyList<EmployeeListResponse>?> GetEmployeesAsync(string? role = null)
    {
        var query = string.IsNullOrWhiteSpace(role) ? string.Empty : $"?role={Uri.EscapeDataString(role)}";
        return await _httpClient.GetFromJsonAsync<List<EmployeeListResponse>>($"api/employees{query}");
    }

    public async Task<EmployeeCardResponse?> GetEmployeeAsync(Guid employeeId)
        => await _httpClient.GetFromJsonAsync<EmployeeCardResponse>($"api/employees/{employeeId}");

    public async Task UpdateEmployeeAsync(Guid employeeId, EmployeeUpdateRequest payload)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/employees/{employeeId}", payload);
        response.EnsureSuccessStatusCode();
    }
}
