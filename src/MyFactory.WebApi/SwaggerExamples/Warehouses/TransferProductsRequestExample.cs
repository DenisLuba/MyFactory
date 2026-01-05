using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class TransferProductsRequestExample : IExamplesProvider<TransferProductsRequest>
{
    public TransferProductsRequest GetExamples() => new(
        FromWarehouseId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
        ToWarehouseId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
        Items: new List<TransferProductItemRequest>
        {
            new(ProductId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"), Qty: 40)
        });
}
