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
                PurchaseNumber: 101m,
                Date: new DateTime(2025, 3, 10),
                Status: PurchaseOrderStatus.Confirmed,
                Items: new List<SupplierPurchaseHistoryItemResponse>
                {
                    new(
                        MaterialType: "Ткань",
                        MaterialName: "Ситец",
                        Qty: 150m,
                        UnitPrice: 180m)
                }),
            new(
                OrderId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                PurchaseNumber: 102m,
                Date: new DateTime(2025, 2, 25),
                Status: PurchaseOrderStatus.Received,
                Items: new List<SupplierPurchaseHistoryItemResponse>
                {
                    new(
                        MaterialType: "Фурнитура",
                        MaterialName: "Молния 20см",
                        Qty: 300m,
                        UnitPrice: 22m)
                })
        });
}
