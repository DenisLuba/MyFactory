using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Reports;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public class ReportsMonthlyProfitResponseExample : IExamplesProvider<ReportsMonthlyProfitResponse>
{
    public ReportsMonthlyProfitResponse GetExamples() =>
        new(
            Period: "11.2025",
            Revenue: 122300m,
            ProductionCost: 78500m,
            Overhead: 18200m,
            Wages: 15000m,
            Profit: 10600m
        );
}

