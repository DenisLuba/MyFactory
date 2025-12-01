using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public class ProductionOrderCreateRequestExample : IExamplesProvider<ProductionOrderCreateRequest>
{
    public ProductionOrderCreateRequest GetExamples() =>
        new(
            ProductName: "Пижама женская",
            Quantity: 150,
            StartDate: new DateTime(2025, 11, 15),
            EndDate: new DateTime(2025, 11, 28),
            Responsible: "Анна Смирнова",
            Status: "Черновик");
}
