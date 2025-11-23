using MyFactory.WebApi.Contracts.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.FinishedGoods;

public class FinishedGoodsInventoryResponseExample : IExamplesProvider<FinishedGoodsInventoryResponse>
{
    public FinishedGoodsInventoryResponse GetExamples() =>
        new(
            SpecificationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            WarehouseId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Quantity: 20.0,
            UnitCost: 444.50m
        );
}
