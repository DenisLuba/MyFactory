using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Reports;

namespace MyFactory.WebApi.SwaggerExamples.Reports;

public class ReportsRevenueResponseExample : IExamplesProvider<IEnumerable<ReportsRevenueResponse>>
{
    public IEnumerable<ReportsRevenueResponse> GetExamples() =>
    [
        new ReportsRevenueResponse(
            SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            SpecificationName: "Пижама женская",
            Revenue: 55000m
        )
    ];
}

