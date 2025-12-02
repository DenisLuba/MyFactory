using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Purchases;

namespace MyFactory.WebApi.SwaggerExamples.Purchases;

public class PurchasesResponseExample : IExamplesProvider<IEnumerable<PurchasesResponse>>
{
    public IEnumerable<PurchasesResponse> GetExamples() =>
    [
        new PurchasesResponse(
            PurchaseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            DocumentNumber: "PR-0001",
            CreatedAt: new DateTime(2025, 11, 12),
            TotalAmount: 16000m,
            ItemsSummary: new[] { "Ткань Ситец (50)", "Молния (100)" },
            Status: PurchasesStatus.Draft
        )
    ];
}

