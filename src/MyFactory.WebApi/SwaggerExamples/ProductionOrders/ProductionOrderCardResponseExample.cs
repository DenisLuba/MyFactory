using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public class ProductionOrderCardResponseExample : IExamplesProvider<ProductionOrderCardResponse>
{
    public ProductionOrderCardResponse GetExamples() =>
        new(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            "PO-001",
            "Пижама женская",
            120,
            new DateTime(2025, 11, 10),
            new DateTime(2025, 11, 20),
            "Анна Смирнова",
            "В работе");
}
