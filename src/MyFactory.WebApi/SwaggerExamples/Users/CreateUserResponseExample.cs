using MyFactory.WebApi.Contracts.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public sealed class CreateUserResponseExample : IExamplesProvider<CreateUserResponse>
{
    public CreateUserResponse GetExamples() => new(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffff0001"));
}
