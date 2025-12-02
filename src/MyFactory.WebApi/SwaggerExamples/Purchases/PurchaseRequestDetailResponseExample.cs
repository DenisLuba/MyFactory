using MyFactory.WebApi.Contracts.Purchases;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Purchases;

public class PurchaseRequestDetailResponseExample : IExamplesProvider<PurchaseRequestDetailResponse>
{
    public PurchaseRequestDetailResponse GetExamples()
    {
        var lines = new[]
        {
            new PurchaseRequestLineResponse(
                LineId: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001"),
                MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                MaterialName: "Ткань Ситец",
                Quantity: 50,
                Unit: "м",
                Price: 250m,
                TotalAmount: 12500m,
                Note: "Основная партия"
            ),
            new PurchaseRequestLineResponse(
                LineId: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002"),
                MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                MaterialName: "Молния",
                Quantity: 100,
                Unit: "шт",
                Price: 35m,
                TotalAmount: 3500m,
                Note: null
            )
        };

        return new PurchaseRequestDetailResponse(
            PurchaseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            DocumentNumber: "PR-0001",
            CreatedAt: new DateTime(2025, 11, 12),
            WarehouseName: "Основной склад",
            SupplierId: Guid.Parse("99999999-0000-0000-0000-000000000001"),
            Comment: "Срочная закупка",
            TotalAmount: lines.Sum(l => l.TotalAmount),
            Status: PurchasesStatus.Draft,
            Items: lines
        );
    }
}
