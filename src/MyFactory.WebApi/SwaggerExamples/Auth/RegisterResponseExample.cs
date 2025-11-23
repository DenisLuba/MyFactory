using MyFactory.WebApi.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Auth;

public class RegisterResponseExample : IExamplesProvider<RegisterResponse>
{
    public RegisterResponse GetExamples() =>
        new(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Status: RegisterStatus.Created
        );
}

