using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Users;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public class UsersCreateRequestExample : IExamplesProvider<UsersCreateRequest>
{
    public UsersCreateRequest GetExamples() =>
        new("john", "john@acme", "Worker", "P@ssw0rd");
}

