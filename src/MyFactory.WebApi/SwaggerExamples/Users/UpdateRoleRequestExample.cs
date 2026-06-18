using MyFactory.WebApi.Contracts.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public sealed class UpdateRoleRequestExample : IExamplesProvider<UpdateRoleRequest>
{
    public UpdateRoleRequest GetExamples() => new(Name: "Dispatcher");
}
