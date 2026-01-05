using MyFactory.Domain.Entities.Inventory;
using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class WarehouseInfoResponseExample : IExamplesProvider<WarehouseInfoResponse>
{
    public WarehouseInfoResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Name: "Склад материалов",
        Type: WarehouseType.Materials);
}
