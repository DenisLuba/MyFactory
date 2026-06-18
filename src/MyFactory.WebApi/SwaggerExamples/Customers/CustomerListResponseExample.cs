using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Customers;

public sealed class CustomerListResponseExample : IExamplesProvider<ListResponse<CustomerListItemResponse>>
{
    public ListResponse<CustomerListItemResponse> GetExamples() => new    (
        [
            new CustomerListItemResponse(
                Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                Name: "��� \"�������\"",
                Phones: ["+7 999 111-22-33"],
                Emails: ["info@rom.ru"],
                Addresses: ["������"],
                IsActive: true),
            new CustomerListItemResponse(
                Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                Name: "�� ������",
                Phones: ["+7 900 555-44-33"],
                Emails: [null],
                Addresses: ["�����"],
                IsActive: true)
        ],
        100,
        30,
        30,
        true
    );
}
