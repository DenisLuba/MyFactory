using MyFactory.WebApi.Contracts.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public sealed class CreateRoleRequestExample : IExamplesProvider<CreateRoleRequest>
{
    public CreateRoleRequest GetExamples() => new(Name: "Operator");
}
