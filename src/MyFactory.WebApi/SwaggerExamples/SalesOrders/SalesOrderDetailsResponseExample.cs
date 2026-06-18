using MyFactory.Domain.Entities.Orders;
using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class SalesOrderDetailsResponseExample : IExamplesProvider<SalesOrderDetailsResponse>
{
    public SalesOrderDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        OrderNumber: 1,
        OrderDate: new DateTime(2025, 3, 15),
        Status: SalesOrderStatus.New,
        Customer: new SalesOrderCustomerDetailsResponse(
            Id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
            Name: "��� \"��������\"",
            Phone: "+7 (900) 111-22-33",
            Email: "info@tex.ru",
            Address: "�. �������, ��. �������������, 12"),
        Items: new List<SalesOrderItemResponse>
        {
            new(
                Id: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"),
                ProductId: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0005"),
                ProductName: "������ �������",
                QtyOrdered: 120m,
                QtyAllocated: 100m,
                QtyShipped: 50m)
        });
}
