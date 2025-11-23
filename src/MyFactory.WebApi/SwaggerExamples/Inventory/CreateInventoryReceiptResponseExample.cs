using MyFactory.WebApi.Contracts.Inventory;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Inventory;

public class CreateInventoryReceiptResponseExample : IExamplesProvider<CreateInventoryReceiptResponse>
{
    public CreateInventoryReceiptResponse GetExamples() =>
        new(
            ReceiptId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Status: StatusInventory.Posted
        );
}
