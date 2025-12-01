using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Customers;

public class CustomerLookupResponseExample : IExamplesProvider<IEnumerable<CustomerLookupResponse>>
{
    public IEnumerable<CustomerLookupResponse> GetExamples() => new[]
    {
        new CustomerLookupResponse(
            CustomerId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name: "ООО \"Текстиль\"",
            Segment: "Сети на северо-западе")
    };
}
