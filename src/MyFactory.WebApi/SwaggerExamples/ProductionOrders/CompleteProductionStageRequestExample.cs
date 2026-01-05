using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class CompleteProductionStageRequestExample : IExamplesProvider<CompleteProductionStageRequest>
{
    public CompleteProductionStageRequest GetExamples() => new(Qty: 120);
}
