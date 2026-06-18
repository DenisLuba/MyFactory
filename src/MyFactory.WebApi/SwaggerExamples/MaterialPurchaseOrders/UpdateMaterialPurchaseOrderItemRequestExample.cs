using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class UpdateMaterialPurchaseOrderItemRequestExample : IExamplesProvider<UpdateMaterialPurchaseOrderItemRequest>
{
    public UpdateMaterialPurchaseOrderItemRequest GetExamples() => new(Qty: 120, UnitPrice: 115);
}
