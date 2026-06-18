using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class UpdateSalesOrderRequestExample : IExamplesProvider<UpdateSalesOrderRequest>
{
    public UpdateSalesOrderRequest GetExamples() => new(
        CustomerId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"),
        OrderDate: new DateTime(2025, 3, 20));
}
