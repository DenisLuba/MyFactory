using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class IssueMaterialsToProductionRequestExample : IExamplesProvider<IssueMaterialsToProductionRequest>
{
    public IssueMaterialsToProductionRequest GetExamples() => new(
        Materials: new List<IssueMaterialLineRequest>
        {
            new(
                MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                WarehouseId: Guid.Parse("99999999-0000-0000-0000-000000000001"),
                Qty: 80),
            new(
                MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                WarehouseId: Guid.Parse("99999999-0000-0000-0000-000000000002"),
                Qty: 120)
        });
}
