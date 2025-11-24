using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Purchases;

namespace MyFactory.WebApi.SwaggerExamples.Purchases;

public class PurchasesResponseExample : IExamplesProvider<IEnumerable<PurchasesResponse>>
{
    public IEnumerable<PurchasesResponse> GetExamples() =>
    [
        new PurchasesResponse(
            PurchaseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            CreatedAt: new DateTime(2025, 11, 12),
            Status: PurchasesStatus.Draft,
            Items:
            [
                new PurchaseResponseItem(
                    MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                    Qty: 50
                )
            ]
        )
    ];
}

