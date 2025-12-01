using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public class ReportsMonthlyProfitListExample : IExamplesProvider<IEnumerable<ReportsMonthlyProfitResponse>>
{
    public IEnumerable<ReportsMonthlyProfitResponse> GetExamples() =>
        new List<ReportsMonthlyProfitResponse>
        {
            new(
                Period: "11.2025",
                Revenue: 122300m,
                ProductionCost: 78500m,
                Overhead: 18200m,
                Wages: 5200m,
                Profit: 20400m
            ),
            new(
                Period: "10.2025",
                Revenue: 115400m,
                ProductionCost: 75900m,
                Overhead: 17900m,
                Wages: 5600m,
                Profit: 16000m
            )
        };
}
