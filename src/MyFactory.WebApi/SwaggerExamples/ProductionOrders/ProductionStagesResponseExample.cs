using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionStagesResponseExample : IExamplesProvider<IReadOnlyList<ProductionStageSummaryResponse>>
{
    public IReadOnlyList<ProductionStageSummaryResponse> GetExamples() => new List<ProductionStageSummaryResponse>
    {
        new(ProductionOrderStatus.Cutting, 60, 60),
        new(ProductionOrderStatus.Sewing, 40, 80),
        new(ProductionOrderStatus.Packaging, 20, 100)
    };
}
