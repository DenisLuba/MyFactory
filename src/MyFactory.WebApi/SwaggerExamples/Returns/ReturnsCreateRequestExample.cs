using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Returns;

namespace MyFactory.WebApi.SwaggerExamples.Returns;

public class ReturnsCreateRequestExample : IExamplesProvider<ReturnsCreateRequest>
{
    public ReturnsCreateRequest GetExamples() =>
        new ReturnsCreateRequest(
            CustomerId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            SpecificationId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Quantity: 2,
            Reason: "Брак — разошёлся шов",
            ReturnDate: new DateTime(2025, 11, 13)
        );
}

