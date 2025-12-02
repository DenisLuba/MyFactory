using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Purchases;

namespace MyFactory.WebApi.SwaggerExamples.Purchases;

public class PurchasesConvertToOrderResponseExample : IExamplesProvider<PurchasesConvertToOrderResponse>
{
    public PurchasesConvertToOrderResponse GetExamples() =>
        new(
            PurchaseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Status: PurchasesStatus.ConvertedToOrder
        );
}

