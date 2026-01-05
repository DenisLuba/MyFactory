using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class AddMaterialToWarehouseRequestExample : IExamplesProvider<AddMaterialToWarehouseRequest>
{
    public AddMaterialToWarehouseRequest GetExamples() => new(
        MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
        Qty: 50m);
}
