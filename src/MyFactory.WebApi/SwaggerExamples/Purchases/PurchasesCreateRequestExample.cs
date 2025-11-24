using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Purchases;

namespace MyFactory.WebApi.SwaggerExamples.Purchases;

public class PurchasesCreateRequestExample : IExamplesProvider<PurchasesCreateRequest>
{
    public PurchasesCreateRequest GetExamples() =>
        new(
            SupplierId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Items:
            [
                new PurchaseItemRequest(
                    MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                    Qty: 50
                )
            ]
        );
}

