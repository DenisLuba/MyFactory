using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class CreateSalesOrderResponseExample : IExamplesProvider<CreateSalesOrderResponse>
{
    public CreateSalesOrderResponse GetExamples() => new(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffff0001"));
}
