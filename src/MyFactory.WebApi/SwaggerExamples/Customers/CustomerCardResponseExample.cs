using MyFactory.Domain.Entities.Orders;
using MyFactory.WebApi.Contracts.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Customers;

public sealed class CustomerCardResponseExample : IExamplesProvider<CustomerCardResponse>
{
    public CustomerCardResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Name: "ООО \"Ромашка\"",
        Phone: "+7 999 111-22-33",
        Email: "info@rom.ru",
        Address: "Москва",
        Orders: new List<CustomerOrderItemResponse>
        {
            new(
                Id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0001"),
                OrderNumber: "SO-0001",
                OrderDate: new DateTime(2025, 3, 12),
                Status: SalesOrderStatus.Confirmed),
            new(
                Id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0002"),
                OrderNumber: "SO-0005",
                OrderDate: new DateTime(2025, 3, 18),
                Status: SalesOrderStatus.New)
        });
}
