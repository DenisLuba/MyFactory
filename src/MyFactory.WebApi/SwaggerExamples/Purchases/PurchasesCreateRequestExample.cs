using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Purchases;

namespace MyFactory.WebApi.SwaggerExamples.Purchases;

public class PurchasesCreateRequestExample : IExamplesProvider<PurchasesCreateRequest>
{
    public PurchasesCreateRequest GetExamples() =>
        new(
            DocumentNumber: "PR-0002",
            CreatedAt: DateTime.UtcNow,
            WarehouseName: "Основной склад",
            SupplierId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Comment: "Закупка к новому заказу",
            Items:
            [
                new PurchaseItemRequest(
                    MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                    MaterialName: "Ткань Ситец",
                    Quantity: 50,
                    Unit: "м",
                    Price: 250m,
                    Note: "Основной цвет"
                )
            ]
        );
}

