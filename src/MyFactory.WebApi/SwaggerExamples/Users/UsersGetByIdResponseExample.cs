using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Users;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public class UsersGetByIdResponseExample : IExamplesProvider<UsersGetByIdResponse>
{
    public UsersGetByIdResponse GetExamples() =>
        new(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "admin",
            "admin@acme",
            "Director",
            true
        );
}

