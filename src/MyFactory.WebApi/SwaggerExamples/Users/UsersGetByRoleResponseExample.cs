using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Users;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public class UsersGetByRoleResponseExample : IExamplesProvider<IEnumerable<UsersGetByRoleResponse>>
{
    public IEnumerable<UsersGetByRoleResponse> GetExamples() =>
    [
        new UsersGetByRoleResponse(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "admin",
            "admin@acme",
            "Director",
            true
        ),
        new UsersGetByRoleResponse(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "keeper",
            "keeper@acme",
            "Storekeeper",
            true
        )
    ];
}

