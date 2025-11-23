using MyFactory.WebApi.Contracts.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.FinishedGoods;

public class ReceiptFinishedGoodsResponseExample : IExamplesProvider<ReceiptFinishedGoodsResponse>
{
    public ReceiptFinishedGoodsResponse GetExamples() =>
        new(
            ReceiptId: "fg-rc-001",
            Status: FinishedGoodsStatus.Moved
        );
}
