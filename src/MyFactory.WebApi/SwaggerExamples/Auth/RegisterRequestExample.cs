using MyFactory.WebApi.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Auth;

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples() =>
        new(
            UserName: "ivanov",
            Email: "i@domain.com",
            Password: "P@ssw0rd123"
        );
}

