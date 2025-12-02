using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public class WarehousesListResponseExample : IExamplesProvider<IEnumerable<WarehousesListResponse>>
{
    public IEnumerable<WarehousesListResponse> GetExamples() => new[]
    {
        new WarehousesListResponse(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "ST-001",
            "Основной склад",
            WarehouseType.Materials,
            WarehouseStatus.Active
        ),
        new WarehousesListResponse(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "ST-002",
            "Склад фурнитуры",
            WarehouseType.Materials,
            WarehouseStatus.Active
        ),
        new WarehousesListResponse(
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            "ST-003",
            "Готовая продукция",
            WarehouseType.FinishedGoods,
            WarehouseStatus.Inactive
        )
    };
}
