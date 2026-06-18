using MyFactory.WebApi.Contracts.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public sealed class CloseMonthlyFinancialReportRequestExample : IExamplesProvider<CloseMonthlyFinancialReportRequest>
{
    public CloseMonthlyFinancialReportRequest GetExamples() => new(Year: 2025, Month: 1);
}
