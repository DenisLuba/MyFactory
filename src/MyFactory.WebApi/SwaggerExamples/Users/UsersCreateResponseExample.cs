using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Users;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public class UsersCreateResponseExample : IExamplesProvider<UsersCreateResponse>
{
    public UsersCreateResponse GetExamples() =>
        new(
            Guid.Parse("99999999-9999-9999-9999-999999999999"),
            UserStatus.Created
        );
}

