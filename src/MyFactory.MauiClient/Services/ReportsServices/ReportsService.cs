using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Reports;

namespace MyFactory.MauiClient.Services.ReportsServices
{
    public class ReportsService(HttpClient httpClient) : IReportsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ReportsMonthlyProfitResponse?> MonthlyProfitAsync(int month, int year)
            => await _httpClient.GetFromJsonAsync<ReportsMonthlyProfitResponse>($"api/reports/monthly-profit?month={month}&year={year}");

        public async Task<List<ReportsRevenueResponse>?> RevenueAsync(int month, int year)
            => await _httpClient.GetFromJsonAsync<List<ReportsRevenueResponse>>($"api/reports/revenue?month={month}&year={year}");

        public async Task<List<ReportsProductionCostResponse>?> ProductionCostAsync(int month, int year)
            => await _httpClient.GetFromJsonAsync<List<ReportsProductionCostResponse>>($"api/reports/production-cost?month={month}&year={year}");
    }
}