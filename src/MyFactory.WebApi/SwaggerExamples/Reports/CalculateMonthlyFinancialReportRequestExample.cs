using MyFactory.WebApi.Contracts.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public sealed class CalculateMonthlyFinancialReportRequestExample : IExamplesProvider<CalculateMonthlyFinancialReportRequest>
{
    public CalculateMonthlyFinancialReportRequest GetExamples() => new(Year: 2025, Month: 3);
}
