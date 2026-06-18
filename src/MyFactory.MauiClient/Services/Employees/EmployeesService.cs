using System.Net.Http.Json;
using System.Web;
using MyFactory.MauiClient.Models.Common;
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

    public async Task<ListResponse<EmployeeListItemResponse>?> GetListByDateAsync(
        string? searchName,
        string? sortBy,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        bool? isActive = null,
        Guid? departmentId = null,
        Guid? positionId = null,
        int? grade = null,
        DateTime? hiredFrom = null,
        DateTime? hiredTo = null,
        bool? canCut = null,
        bool? canSew = null,
        bool? canPackage = null,
        ICollection<Guid>? exceptEmployeeIds = null)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(searchName))
            query.Add($"searchName={searchName}");

        if (!string.IsNullOrWhiteSpace(sortBy))
            query.Add($"sortBy={Uri.EscapeDataString(sortBy)}");

        if (sortDesc)
            query.Add("sortDesc=true");

        query.Add($"skip={skip}");

        query.Add($"take={take}");

        if (isActive.HasValue)      
            query.Add($"isActive={isActive.Value}");

        if (departmentId.HasValue && departmentId.Value != Guid.Empty)
            query.Add($"departmentId={departmentId}");

        if (positionId.HasValue && positionId.Value != Guid.Empty)
            query.Add($"positionId={positionId}");

        if (grade.HasValue)
            query.Add($"grade={grade}");

        if (hiredFrom is not null)
            query.Add($"hiredFrom={hiredFrom:yyyy-MM-dd}");

        if (hiredTo is not null)
            query.Add($"hiredTo={hiredTo:yyyy-MM-dd}");

        if (canCut.HasValue)
            query.Add($"canCut={canCut.Value}");

        if (canSew.HasValue)
            query.Add($"canSew={canSew.Value}");

        if (canPackage.HasValue)
            query.Add($"canPackage={canPackage.Value}");

        if (exceptEmployeeIds is not null && exceptEmployeeIds.Count > 0)
            query.AddRange(exceptEmployeeIds.Where(id => id != Guid.Empty).Select(id => $"exceptEmployeeIds={id}"));

        var path = "api/employees" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);

        return await _httpClient.GetFromJsonAsync<ListResponse<EmployeeListItemResponse>>(path);
    }

    public async Task<ListResponse<EmployeeListItemResponse>?> GetListAsync(
        string? searchName = null,
        string? searchType = null,
        string? sortBy = null,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        bool? isActive = null,
        Guid? fromId = null)
    {
        return await GetListByDateAsync(
            searchName: searchName,
            sortBy: sortBy,
            sortDesc: sortDesc,
            skip: skip,
            take: take,
            isActive: isActive,
            departmentId: fromId
        );
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
