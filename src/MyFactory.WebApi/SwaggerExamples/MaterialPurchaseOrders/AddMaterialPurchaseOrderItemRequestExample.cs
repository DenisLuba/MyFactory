using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class AddMaterialPurchaseOrderItemRequestExample : IExamplesProvider<AddMaterialPurchaseOrderItemRequest>
{
    public AddMaterialPurchaseOrderItemRequest GetExamples() => new(
        MaterialId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
        Qty: 100,
        UnitPrice: 120);
}
