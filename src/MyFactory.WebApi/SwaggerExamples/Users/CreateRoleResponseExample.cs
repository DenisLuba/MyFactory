using MyFactory.WebApi.Contracts.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public sealed class CreateRoleResponseExample : IExamplesProvider<CreateRoleResponse>
{
    public CreateRoleResponse GetExamples() => new(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffff0002"));
}
