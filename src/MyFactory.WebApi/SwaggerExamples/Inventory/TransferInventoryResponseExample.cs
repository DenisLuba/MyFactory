using MyFactory.WebApi.Contracts.Inventory;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Inventory;

public class TransferInventoryResponseExample : IExamplesProvider<TransferInventoryResponse>
{
    public TransferInventoryResponse GetExamples() =>
        new(Status: StatusInventory.Transferred);
}
