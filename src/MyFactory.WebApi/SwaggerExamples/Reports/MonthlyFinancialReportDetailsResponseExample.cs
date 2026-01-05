using MyFactory.Domain.Entities.Finance;
using MyFactory.WebApi.Contracts.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public sealed class MonthlyFinancialReportDetailsResponseExample : IExamplesProvider<MonthlyFinancialReportDetailsResponse>
{
    public MonthlyFinancialReportDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Year: 2025,
        Month: 3,
        TotalRevenue: 250000m,
        PayrollExpenses: 90000m,
        MaterialExpenses: 70000m,
        OtherExpenses: 15000m,
        Profit: 75000m,
        Status: MonthlyReportStatus.Calculated,
        CalculatedAt: new DateTime(2025, 4, 5, 12, 30, 0, DateTimeKind.Utc),
        CreatedBy: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"));
}
