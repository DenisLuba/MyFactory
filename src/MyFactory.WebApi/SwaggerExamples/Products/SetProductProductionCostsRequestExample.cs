using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class SetProductProductionCostsRequestExample : IExamplesProvider<SetProductProductionCostsRequest>
{
    public SetProductProductionCostsRequest GetExamples() => new(
        Costs: new List<ProductDepartmentCostRequest>
        {
            new(
                DepartmentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                CutCost: 50m,
                SewingCost: 120m,
                PackCost: 30m,
                Expenses: 20m)
        });
}
