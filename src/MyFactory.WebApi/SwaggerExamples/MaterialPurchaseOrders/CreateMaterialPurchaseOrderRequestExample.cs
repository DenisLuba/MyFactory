using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class CreateMaterialPurchaseOrderRequestExample : IExamplesProvider<CreateMaterialPurchaseOrderRequest>
{
    public CreateMaterialPurchaseOrderRequest GetExamples() => new(
        SupplierId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        OrderDate: new DateTime(2025, 3, 10));
}
