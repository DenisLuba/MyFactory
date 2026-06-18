using MyFactory.WebApi.Contracts.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Customers;

public sealed class CreateCustomerResponseExample : IExamplesProvider<CreateCustomerResponse>
{
    public CreateCustomerResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"));
}
