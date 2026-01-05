using MyFactory.WebApi.Contracts.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public sealed class ApproveMonthlyFinancialReportRequestExample : IExamplesProvider<ApproveMonthlyFinancialReportRequest>
{
    public ApproveMonthlyFinancialReportRequest GetExamples() => new(Year: 2025, Month: 2);
}
