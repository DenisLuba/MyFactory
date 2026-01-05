using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class UpdateWarehouseMaterialQtyRequestExample : IExamplesProvider<UpdateWarehouseMaterialQtyRequest>
{
    public UpdateWarehouseMaterialQtyRequest GetExamples() => new(Qty: 175m);
}
