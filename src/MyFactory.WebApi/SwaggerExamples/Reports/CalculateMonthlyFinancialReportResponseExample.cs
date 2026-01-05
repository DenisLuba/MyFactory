using MyFactory.WebApi.Contracts.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public sealed class CalculateMonthlyFinancialReportResponseExample : IExamplesProvider<CalculateMonthlyFinancialReportResponse>
{
    public CalculateMonthlyFinancialReportResponse GetExamples() => new(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffff0001"));
}
