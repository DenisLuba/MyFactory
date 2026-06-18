using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class AddSalesOrderItemRequestExample : IExamplesProvider<AddSalesOrderItemRequest>
{
    public AddSalesOrderItemRequest GetExamples() => new(
        ProductId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
        Qty: 100m);
}
