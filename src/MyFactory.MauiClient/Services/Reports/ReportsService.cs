using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Reports;

namespace MyFactory.MauiClient.Services.Reports;

public sealed class ReportsService : IReportsService
{
    private readonly HttpClient _httpClient;

    public ReportsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<MonthlyFinancialReportListItemResponse>?> GetMonthlyAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<MonthlyFinancialReportListItemResponse>>("api/reports/monthly");
    }

    public async Task<MonthlyFinancialReportDetailsResponse?> GetMonthlyDetailsAsync(int year, int month)
    {
        return await _httpClient.GetFromJsonAsync<MonthlyFinancialReportDetailsResponse>($"api/reports/monthly/{year}/{month}");
    }

    public async Task<CalculateMonthlyFinancialReportResponse?> CalculateAsync(CalculateMonthlyFinancialReportRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reports/monthly/calculate", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CalculateMonthlyFinancialReportResponse>();
    }

    public async Task RecalculateAsync(RecalculateMonthlyFinancialReportRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reports/monthly/recalculate", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task ApproveAsync(ApproveMonthlyFinancialReportRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reports/monthly/approve", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CloseAsync(CloseMonthlyFinancialReportRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reports/monthly/close", request);
        response.EnsureSuccessStatusCode();
    }
}
