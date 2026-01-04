using MyFactory.WebApi.Contracts.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Customers;

public sealed class CustomerListResponseExample : IExamplesProvider<IReadOnlyList<CustomerListItemResponse>>
{
    public IReadOnlyList<CustomerListItemResponse> GetExamples() => new List<CustomerListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            Name: "ООО \"Ромашка\"",
            Phone: "+7 999 111-22-33",
            Email: "info@rom.ru",
            Address: "Москва"),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            Name: "ИП Иванов",
            Phone: "+7 900 555-44-33",
            Email: null,
            Address: "Тверь")
    };
}
