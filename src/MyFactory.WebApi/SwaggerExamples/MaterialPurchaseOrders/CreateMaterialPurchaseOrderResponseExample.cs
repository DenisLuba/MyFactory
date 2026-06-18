using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class CreateMaterialPurchaseOrderResponseExample : IExamplesProvider<CreateMaterialPurchaseOrderResponse>
{
    public CreateMaterialPurchaseOrderResponse GetExamples() => new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"));
}
