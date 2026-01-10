using System.Net.Http.Json;
using System.Web;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Employees;

public sealed class EmployeesService : IEmployeesService
{
    private readonly HttpClient _httpClient;

    public EmployeesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<EmployeeListItemResponse>?> GetListAsync(string? search = null, EmployeeSortBy sortBy = EmployeeSortBy.FullName, bool sortDesc = false)
    {
        var builder = new UriBuilder(new Uri(_httpClient.BaseAddress!, "api/employees"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrWhiteSpace(search))
        {
            query["search"] = search;
        }
        if (sortBy != EmployeeSortBy.FullName)
        {
            query["sortBy"] = sortBy.ToString();
        }
        if (sortDesc)
        {
            query["sortDesc"] = "true";
        }
        var queryString = query.ToString();
        if (!string.IsNullOrEmpty(queryString))
        {
            builder.Query = queryString;
        }
        return await _httpClient.GetFromJsonAsync<List<EmployeeListItemResponse>>(builder.Uri);
    }

    public async Task<EmployeeDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<EmployeeDetailsResponse>($"api/employees/{id}");
    }

    public async Task<CreateEmployeeResponse?> CreateAsync(CreateEmployeeRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/employees", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateEmployeeResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateEmployeeRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/employees/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task ActivateAsync(Guid id, ActivateEmployeeRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/employees/{id}/activate", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task DeactivateAsync(Guid id, DeactivateEmployeeRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/employees/{id}/deactivate", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<IReadOnlyList<EmployeeProductionAssignmentResponse>?> GetAssignmentsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<List<EmployeeProductionAssignmentResponse>>($"api/employees/{id}/assignments");
    }

    public async Task<IReadOnlyList<TimesheetListItemResponse>?> GetTimesheetsAsync(Guid? employeeId, Guid? departmentId, int year, int month)
    {
        var builder = new UriBuilder(new Uri(_httpClient.BaseAddress!, "api/employees/timesheets"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (employeeId.HasValue)
        {
            query["employeeId"] = employeeId.Value.ToString();
        }
        if (departmentId.HasValue)
        {
            query["departmentId"] = departmentId.Value.ToString();
        }
        query["year"] = year.ToString();
        query["month"] = month.ToString();
        builder.Query = query.ToString();
        return await _httpClient.GetFromJsonAsync<List<TimesheetListItemResponse>>(builder.Uri);
    }

    public async Task<IReadOnlyList<EmployeeTimesheetEntryResponse>?> GetEmployeeTimesheetAsync(Guid id, int year, int month)
    {
        var builder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"api/employees/{id}/timesheet"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["year"] = year.ToString();
        query["month"] = month.ToString();
        builder.Query = query.ToString();
        return await _httpClient.GetFromJsonAsync<List<EmployeeTimesheetEntryResponse>>(builder.Uri);
    }

    public async Task<AddTimesheetEntryResponse?> AddTimesheetEntryAsync(Guid id, AddTimesheetEntryRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/employees/{id}/timesheet", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<AddTimesheetEntryResponse>();
    }

    public async Task UpdateTimesheetEntryAsync(Guid entryId, UpdateTimesheetEntryRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/employees/timesheet/{entryId}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task RemoveTimesheetEntryAsync(Guid entryId)
    {
        var response = await _httpClient.DeleteAsync($"api/employees/timesheet/{entryId}");
        await response.EnsureSuccessWithProblemAsync();
    }
}
