using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Warehouses;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public class WarehousesCreateRequestExample : IExamplesProvider<WarehousesCreateRequest>
{
    public WarehousesCreateRequest GetExamples() =>
        new("Основной склад", WarehouseType.Materials, "ул. Заводская, 1");
}

