using MyFactory.WebApi.Contracts.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public sealed class SupplierDetailsResponseExample : IExamplesProvider<SupplierDetailsResponse>
{
    public SupplierDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Name: "ТексМаркет",
        Description: "Оптовый поставщик тканей",
        Purchases: new List<SupplierPurchaseHistoryResponse>
        {
            new(
                MaterialType: "Ткань",
                MaterialName: "Ситец",
                Qty: 150m,
                UnitPrice: 180m,
                Date: new DateTime(2025, 3, 10)),
            new(
                MaterialType: "Фурнитура",
                MaterialName: "Молния 20см",
                Qty: 300m,
                UnitPrice: 22m,
                Date: new DateTime(2025, 2, 25))
        });
}
