using MyFactory.Domain.Entities.Orders;
using MyFactory.WebApi.Contracts.SalesOrders;
using MyFactory.WebApi.Contracts.Common;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class SalesOrderListResponseExample : IExamplesProvider<ListResponse<SalesOrderListItemResponse>>
{
    public ListResponse<SalesOrderListItemResponse> GetExamples() => new ListResponse<SalesOrderListItemResponse>
    (
        Items : 
        [
            new(
                Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                OrderNumber: 1,
                CustomerName: "Закзчик 1",
                OrderDate: new DateTime(2025, 3, 15),
                Status: SalesOrderStatus.New),
            new(
                Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                OrderNumber: 2,
                CustomerName: "",
                OrderDate: new DateTime(2025, 3, 18),
                Status: SalesOrderStatus.Confirmed)
        ],
        TotalCount : 2,
        Take : 30,
        Skip : 0,
        HasMore : false
    );
}
