using MyFactory.WebApi.Contracts.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Customers;

public sealed class CustomerDetailsResponseExample : IExamplesProvider<CustomerDetailsResponse>
{
    public CustomerDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Name: "ООО \"Ромашка\"",
        Phone: "+7 999 111-22-33",
        Email: "info@rom.ru",
        Address: "Москва, ул. Пример, 1");
}
