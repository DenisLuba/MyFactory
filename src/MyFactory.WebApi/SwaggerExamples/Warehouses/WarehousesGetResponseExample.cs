using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Warehouses;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public class WarehousesGetResponseExample : IExamplesProvider<WarehousesGetResponse>
{
    public WarehousesGetResponse GetExamples() =>
        new(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "ST-001",
            "Основной склад",
            WarehouseType.Materials,
            "ул. Заводская, 1",
            WarehouseStatus.Active
        );
}

