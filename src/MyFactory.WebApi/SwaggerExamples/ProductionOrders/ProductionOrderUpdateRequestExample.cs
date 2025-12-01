using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public class ProductionOrderUpdateRequestExample : IExamplesProvider<ProductionOrderUpdateRequest>
{
    public ProductionOrderUpdateRequest GetExamples() =>
        new(
            ProductName: "Пижама женская",
            Quantity: 120,
            StartDate: new DateTime(2025, 11, 10),
            EndDate: new DateTime(2025, 11, 22),
            Responsible: "Анна Смирнова",
            Status: "В работе");
}
