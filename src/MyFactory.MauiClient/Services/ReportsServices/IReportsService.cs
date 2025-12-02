using System.Collections.Generic;
using MyFactory.MauiClient.Models.Reports;

namespace MyFactory.MauiClient.Services.ReportsServices
{
    public interface IReportsService
    {
        Task<List<ReportsMonthlyProfitResponse>?> GetMonthlyProfitByYearAsync(int year);
        // Task<ReportsMonthlyProfitResponse?> MonthlyProfitAsync(int month, int year);
        // Task<List<ReportsRevenueResponse>?> RevenueAsync(int month, int year);
        // Task<List<ReportsProductionCostResponse>?> ProductionCostAsync(int month, int year);
    }
}