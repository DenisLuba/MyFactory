using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public class ProductionOrderListResponseExample : IExamplesProvider<IEnumerable<ProductionOrderListResponse>>
{
    public IEnumerable<ProductionOrderListResponse> GetExamples() =>
    [
        new(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            "PO-001",
            "Пижама женская",
            120,
            new DateTime(2025, 11, 10),
            new DateTime(2025, 11, 20),
            "В работе"),
        new(
            Guid.Parse("10000000-0000-0000-0000-000000000002"),
            "PO-002",
            "Халат махровый",
            80,
            new DateTime(2025, 11, 12),
            new DateTime(2025, 11, 25),
            "Черновик")
    ];
}
