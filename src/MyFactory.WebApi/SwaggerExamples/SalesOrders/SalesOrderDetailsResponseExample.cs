using MyFactory.Domain.Entities.Orders;
using MyFactory.WebApi.Contracts.SalesOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.SalesOrders;

public sealed class SalesOrderDetailsResponseExample : IExamplesProvider<SalesOrderDetailsResponse>
{
    public SalesOrderDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        OrderNumber: "SO-001",
        OrderDate: new DateTime(2025, 3, 15),
        Status: SalesOrderStatus.New,
        Customer: new SalesOrderCustomerDetailsResponse(
            Id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
            Name: "ООО \"Текстиль\"",
            Phone: "+7 (900) 111-22-33",
            Email: "info@tex.ru",
            Address: "г. Иваново, ул. Текстильщиков, 12"),
        Items: new List<SalesOrderItemResponse>
        {
            new(
                Id: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"),
                ProductId: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0005"),
                ProductName: "Пижама женская",
                QtyOrdered: 120m)
        });
}
