using MyFactory.WebApi.Contracts.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.FinishedGoods;

public class MoveFinishedGoodsRequestExample : IExamplesProvider<MoveFinishedGoodsRequest>
{
    public MoveFinishedGoodsRequest GetExamples() =>
        new(
            SpecificationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FromWarehouseId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ToWarehouseId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Quantity: 10,
            Reason: "Перемещение в зону отгрузки для выполнения заказа"
        );
}
