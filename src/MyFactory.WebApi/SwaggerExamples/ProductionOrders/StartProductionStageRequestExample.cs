using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class StartProductionStageRequestExample : IExamplesProvider<StartProductionStageRequest>
{
    public StartProductionStageRequest GetExamples() => new(ProductionOrderStatus.Cutting);
}
