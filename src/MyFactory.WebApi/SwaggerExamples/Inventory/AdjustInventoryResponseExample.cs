using MyFactory.WebApi.Contracts.Inventory;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Inventory;

public class AdjustInventoryResponseExample : IExamplesProvider<AdjustInventoryResponse>
{
    public AdjustInventoryResponse GetExamples() =>
        new(Status: StatusInventory.Adjusted);
}
