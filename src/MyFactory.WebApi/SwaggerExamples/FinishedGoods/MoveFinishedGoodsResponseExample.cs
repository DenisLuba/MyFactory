using MyFactory.WebApi.Contracts.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.FinishedGoods;

public class MoveFinishedGoodsResponseExample : IExamplesProvider<MoveFinishedGoodsResponse>
{
    public MoveFinishedGoodsResponse GetExamples() =>
        new(Status: FinishedGoodsStatus.Moved);
}
