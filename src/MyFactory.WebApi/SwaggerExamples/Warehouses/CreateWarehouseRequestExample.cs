using MyFactory.Domain.Entities.Inventory;
using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class CreateWarehouseRequestExample : IExamplesProvider<CreateWarehouseRequest>
{
    public CreateWarehouseRequest GetExamples() => new(
        Name: "Склад материалов",
        Type: WarehouseType.Materials);
}
