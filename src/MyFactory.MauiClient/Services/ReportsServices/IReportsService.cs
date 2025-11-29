using MyFactory.MauiClient.Models.Reports;

namespace MyFactory.MauiClient.Services.ReportsServices
{
    public interface IReportsService
    {
        Task<ReportsMonthlyProfitResponse?> MonthlyProfitAsync(int month, int year);
        Task<List<ReportsRevenueResponse>?> RevenueAsync(int month, int year);
        Task<List<ReportsProductionCostResponse>?> ProductionCostAsync(int month, int year);
    }
}