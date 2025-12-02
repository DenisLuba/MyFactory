using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Warehouses;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public class WarehousesCreateRequestExample : IExamplesProvider<WarehousesCreateRequest>
{
    public WarehousesCreateRequest GetExamples() =>
    new("ST-010", "Новый склад", WarehouseType.Materials, "ул. Новая, 5", WarehouseStatus.Active);
}

