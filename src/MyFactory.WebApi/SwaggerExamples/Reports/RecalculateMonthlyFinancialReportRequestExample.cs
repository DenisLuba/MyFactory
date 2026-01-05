using MyFactory.WebApi.Contracts.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public sealed class RecalculateMonthlyFinancialReportRequestExample : IExamplesProvider<RecalculateMonthlyFinancialReportRequest>
{
    public RecalculateMonthlyFinancialReportRequest GetExamples() => new(Year: 2025, Month: 3);
}
