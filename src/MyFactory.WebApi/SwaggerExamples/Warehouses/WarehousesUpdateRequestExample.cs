using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Warehouses;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public class WarehousesUpdateRequestExample : IExamplesProvider<WarehousesUpdateRequest>
{
    public WarehousesUpdateRequest GetExamples() =>
        new("Основной склад (обновлён)", WarehouseType.Materials, "ул. Заводская, 1", WarehouseStatus.Active);
}

