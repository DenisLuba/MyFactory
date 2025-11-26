using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Users;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public class UsersUpdateRequestExample : IExamplesProvider<UsersUpdateRequest>
{
    public UsersUpdateRequest GetExamples() =>
        new(
            "admin",
            "admin@acme",
            "Director",
            true
        );
}

