using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class TransferMaterialsRequestExample : IExamplesProvider<TransferMaterialsRequest>
{
    public TransferMaterialsRequest GetExamples() => new(
        FromWarehouseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        ToWarehouseId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
        Items: new List<TransferMaterialItemRequest>
        {
            new(MaterialId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"), Qty: 25m)
        });
}
