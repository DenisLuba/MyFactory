using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class UpdateSalesOrderItemRequestExample : IExamplesProvider<UpdateSalesOrderItemRequest>
{
    public UpdateSalesOrderItemRequest GetExamples() => new(Qty: 150m);
}
