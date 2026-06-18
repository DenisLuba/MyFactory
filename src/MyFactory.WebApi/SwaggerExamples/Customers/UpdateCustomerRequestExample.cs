using MyFactory.WebApi.Contracts.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Customers;

public sealed class UpdateCustomerRequestExample : IExamplesProvider<UpdateCustomerRequest>
{
    public UpdateCustomerRequest GetExamples() => new(
        Name: "ООО \"Ромашка\"",
        Phone: "+7 999 111-22-33",
        Email: "support@rom.ru",
        Address: "Москва, ул. Цветочная, 12");
}
