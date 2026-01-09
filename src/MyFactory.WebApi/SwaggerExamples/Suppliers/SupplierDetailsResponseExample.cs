using MyFactory.Domain.Entities.Materials;
using MyFactory.WebApi.Contracts.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public sealed class SupplierDetailsResponseExample : IExamplesProvider<SupplierDetailsResponse>
{
    public SupplierDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Name: "Текстиль+",
        Description: "Надёжный поставщик тканей",
        Purchases: new List<SupplierPurchaseHistoryResponse>
        {
            new(
                OrderId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0001"),
                MaterialType: "Ткань",
                MaterialName: "Ситец",
                Qty: 150m,
                UnitPrice: 180m,
                Date: new DateTime(2025, 3, 10),
                Status: PurchaseOrderStatus.Confirmed),
            new(
                OrderId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                MaterialType: "Фурнитура",
                MaterialName: "Молния 20см",
                Qty: 300m,
                UnitPrice: 22m,
                Date: new DateTime(2025, 2, 25),
                Status: PurchaseOrderStatus.Received)
        });
}
