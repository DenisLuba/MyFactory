using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class UpdateWarehouseProductQtyRequestExample : IExamplesProvider<UpdateWarehouseProductQtyRequest>
{
    public UpdateWarehouseProductQtyRequest GetExamples() => new(QtyPerPackage: 175m, PackageCount: 7m);
}
