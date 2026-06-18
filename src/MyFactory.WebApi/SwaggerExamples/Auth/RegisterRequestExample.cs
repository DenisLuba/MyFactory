using MyFactory.WebApi.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Auth;

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples() =>
        new(
            UserName: "ivanov",
            RoleId: Guid.Parse("aaaaaaaa-aa00-4000-8000-aaaaaaaa0001"),
            Password: "P@ssw0rd123"
        );
}
