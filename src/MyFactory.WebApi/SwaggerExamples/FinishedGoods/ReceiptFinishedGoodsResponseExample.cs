using MyFactory.WebApi.Contracts.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.FinishedGoods;

public class ReceiptFinishedGoodsResponseExample : IExamplesProvider<ReceiptFinishedGoodsResponse>
{
    public ReceiptFinishedGoodsResponse GetExamples() =>
        new(
            ReceiptId: Guid.Parse("11111111-1111-1111-1111-111111111222"),
            Status: FinishedGoodsStatus.Moved
        );
}
