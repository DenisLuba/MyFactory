using MyFactory.MauiClient.Models.Reports;

namespace MyFactory.MauiClient.Services.Reports;

public interface IReportsService
{
    Task<IReadOnlyList<MonthlyFinancialReportListItemResponse>?> GetMonthlyAsync();
    Task<MonthlyFinancialReportDetailsResponse?> GetMonthlyDetailsAsync(int year, int month);
    Task<CalculateMonthlyFinancialReportResponse?> CalculateAsync(CalculateMonthlyFinancialReportRequest request);
    Task RecalculateAsync(RecalculateMonthlyFinancialReportRequest request);
    Task ApproveAsync(ApproveMonthlyFinancialReportRequest request);
    Task CloseAsync(CloseMonthlyFinancialReportRequest request);
}
