using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Purchases;

namespace MyFactory.WebApi.SwaggerExamples.Purchases;

public class PurchasesCreateResponseExample : IExamplesProvider<PurchasesCreateResponse>
{
    public PurchasesCreateResponse GetExamples() =>
        new(
            PurchaseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Status: PurchasesStatus.Created
        );
}

