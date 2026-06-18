using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class CreateSalesOrderRequestExample : IExamplesProvider<CreateSalesOrderRequest>
{
    public CreateSalesOrderRequest GetExamples() => new(
        CustomerId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        OrderDate: new DateTime(2025, 3, 15));
}
