using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Reports;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public class ReportsProductionCostResponseExample : IExamplesProvider<IEnumerable<ReportsProductionCostResponse>>
{
    public IEnumerable<ReportsProductionCostResponse> GetExamples() =>
    [
        new ReportsProductionCostResponse(
            ProductionBatchId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Cost: 78500m
        )
    ];
}

