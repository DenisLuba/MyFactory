using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class CreateWarehouseResponseExample : IExamplesProvider<CreateWarehouseResponse>
{
    public CreateWarehouseResponse GetExamples() => new(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffff0001"));
}
