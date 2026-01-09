using MyFactory.Domain.Entities.Materials;
using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class SupplierPurchaseOrderListItemResponseExample : IExamplesProvider<SupplierPurchaseOrderListItemResponse>
{
    public SupplierPurchaseOrderListItemResponse GetExamples() => new(
        Id: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0001"),
        OrderDate: new DateTime(2024, 1, 12),
        Status: PurchaseOrderStatus.Confirmed,
        ItemsCount: 2,
        TotalAmount: 24000m);
}
