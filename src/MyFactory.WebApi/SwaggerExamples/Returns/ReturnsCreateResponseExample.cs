using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Returns;

namespace MyFactory.WebApi.SwaggerExamples.Returns;

public class ReturnsCreateResponseExample : IExamplesProvider<ReturnsCreateResponse>
{
    public ReturnsCreateResponse GetExamples() =>
        new ReturnsCreateResponse(
            ReturnId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Status: ReturnStatus.Accepted
        );
}

