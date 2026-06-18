using MyFactory.WebApi.Contracts.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public sealed class CreateUserRequestExample : IExamplesProvider<CreateUserRequest>
{
    public CreateUserRequest GetExamples() => new(
        Username: "newuser",
        Password: "P@ssw0rd",
        RoleId: Guid.Parse("11111111-1111-1111-1111-111111111111"));
}
