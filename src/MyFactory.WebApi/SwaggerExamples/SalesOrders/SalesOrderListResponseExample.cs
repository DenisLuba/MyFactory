using MyFactory.Domain.Entities.Orders;
using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class SalesOrderListResponseExample : IExamplesProvider<IReadOnlyList<SalesOrderListItemResponse>>
{
    public IReadOnlyList<SalesOrderListItemResponse> GetExamples() => new List<SalesOrderListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            OrderNumber: "SO-001",
            CustomerName: "ООО \"Текстиль\"",
            OrderDate: new DateTime(2025, 3, 15),
            Status: SalesOrderStatus.New),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            OrderNumber: "SO-002",
            CustomerName: "ИП Клиент",
            OrderDate: new DateTime(2025, 3, 18),
            Status: SalesOrderStatus.Confirmed)
    };
}
