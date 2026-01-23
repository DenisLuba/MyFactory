using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class AddProductToWarehouseRequestExample : IExamplesProvider<AddProductToWarehouseRequest>
{
    public AddProductToWarehouseRequest GetExamples() => new(
        ProductId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0001"),
        Qty: 25);
}
