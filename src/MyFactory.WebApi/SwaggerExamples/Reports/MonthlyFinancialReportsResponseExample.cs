using MyFactory.Domain.Entities.Finance;
using MyFactory.WebApi.Contracts.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public sealed class MonthlyFinancialReportsResponseExample : IExamplesProvider<IReadOnlyList<MonthlyFinancialReportListItemResponse>>
{
    public IReadOnlyList<MonthlyFinancialReportListItemResponse> GetExamples() => new List<MonthlyFinancialReportListItemResponse>
    {
        new(
            Year: 2025,
            Month: 3,
            TotalRevenue: 250000m,
            PayrollExpenses: 90000m,
            MaterialExpenses: 70000m,
            OtherExpenses: 15000m,
            Profit: 75000m,
            Status: MonthlyReportStatus.Calculated),
        new(
            Year: 2025,
            Month: 2,
            TotalRevenue: 200000m,
            PayrollExpenses: 85000m,
            MaterialExpenses: 65000m,
            OtherExpenses: 12000m,
            Profit: 38000m,
            Status: MonthlyReportStatus.Approved)
    };
}
