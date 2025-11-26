using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Warehouses;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public class WarehousesGetResponseExample : IExamplesProvider<IEnumerable<WarehousesGetResponse>>
{
    public IEnumerable<WarehousesGetResponse> GetExamples() =>
    [
        new WarehousesGetResponse(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Основной склад",
            WarehouseType.Materials,
            "ул. Заводская, 1"
        ),
        new WarehousesGetResponse(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Склад ГП",
            WarehouseType.FinishedGoods,
            "ул. Заводская, 2"
        )
    ];
}

