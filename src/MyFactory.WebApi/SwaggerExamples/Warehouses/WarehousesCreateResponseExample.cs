using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Warehouses;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public class WarehousesCreateResponseExample : IExamplesProvider<WarehousesCreateResponse>
{
    public WarehousesCreateResponse GetExamples() =>
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), WarehouseStatus.Active);
}

