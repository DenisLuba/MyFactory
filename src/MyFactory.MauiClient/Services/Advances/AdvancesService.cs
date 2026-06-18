using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Advances;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Advances;

public sealed class AdvancesService : IAdvancesService
{
    private readonly HttpClient _httpClient;

    public AdvancesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CashAdvanceListItemResponse>?> GetListAsync(DateOnly? from = null, DateOnly? to = null, Guid? employeeId = null)
    {
        var queryParts = new List<string>();

        if (from.HasValue)
        {
            queryParts.Add($"from={from.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
        }

        if (to.HasValue)
        {
            queryParts.Add($"to={to.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
        }

        if (employeeId.HasValue)
        {
            queryParts.Add($"employeeId={employeeId.Value}");
        }

        var url = "api/advances";
        if (queryParts.Count > 0)
        {
            url = $"{url}?{string.Join("&", queryParts)}";
        }

        return await _httpClient.GetFromJsonAsync<List<CashAdvanceListItemResponse>>(url);
    }

    public async Task<CreateCashAdvanceResponse?> IssueAsync(CreateCashAdvanceRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/advances/issue", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateCashAdvanceResponse>();
    }

    public async Task AddAmountAsync(Guid cashAdvanceId, AddCashAdvanceAmountRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/advances/{cashAdvanceId}/amount", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<CreateCashAdvanceExpenseResponse?> AddExpenseAsync(Guid cashAdvanceId, CreateCashAdvanceExpenseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/advances/{cashAdvanceId}/expenses", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateCashAdvanceExpenseResponse>();
    }

    public async Task<CreateCashAdvanceReturnResponse?> AddReturnAsync(Guid cashAdvanceId, CreateCashAdvanceReturnRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/advances/{cashAdvanceId}/returns", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateCashAdvanceReturnResponse>();
    }

    public async Task CloseAsync(Guid cashAdvanceId)
    {
        var response = await _httpClient.PostAsync($"api/advances/{cashAdvanceId}/close", null);
        await response.EnsureSuccessWithProblemAsync();
    }
}
