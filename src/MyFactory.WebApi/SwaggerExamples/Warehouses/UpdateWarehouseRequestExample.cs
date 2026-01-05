using MyFactory.Domain.Entities.Inventory;
using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class UpdateWarehouseRequestExample : IExamplesProvider<UpdateWarehouseRequest>
{
    public UpdateWarehouseRequest GetExamples() => new(
        Name: "Склад материалов (обновл.)",
        Type: WarehouseType.Materials);
}
