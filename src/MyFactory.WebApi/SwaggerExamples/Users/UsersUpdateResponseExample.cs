using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Users;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public class UsersUpdateResponseExample : IExamplesProvider<UsersUpdateResponse>
{
    public UsersUpdateResponse GetExamples() =>
        new(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            UserStatus.Updated
        );
}

