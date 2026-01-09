using MyFactory.Domain.Entities.Materials;
using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;

public sealed class MaterialPurchaseOrderDetailsResponseExample : IExamplesProvider<MaterialPurchaseOrderDetailsResponse>
{
    public MaterialPurchaseOrderDetailsResponse GetExamples()
    {
        var items = new List<MaterialPurchaseOrderItemResponse>
        {
            new(
                Id: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0001"),
                MaterialId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
                MaterialName: "Ситец",
                UnitCode: "м",
                Qty: 100,
                UnitPrice: 120),
            new(
                Id: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0002"),
                MaterialId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0004"),
                MaterialName: "Молния",
                UnitCode: "шт",
                Qty: 50,
                UnitPrice: 15)
        };

        return new MaterialPurchaseOrderDetailsResponse(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0001"),
            SupplierId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            SupplierName: "Текстиль+",
            OrderDate: new DateTime(2024, 1, 12),
            Status: PurchaseOrderStatus.Confirmed,
            Items: items);
    }
}
