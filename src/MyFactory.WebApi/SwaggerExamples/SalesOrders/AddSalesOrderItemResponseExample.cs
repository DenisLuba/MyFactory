using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class AddSalesOrderItemResponseExample : IExamplesProvider<AddSalesOrderItemResponse>
{
    public AddSalesOrderItemResponse GetExamples() => new(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"));
}
