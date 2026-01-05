using MyFactory.WebApi.Contracts.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public sealed class RoleListResponseExample : IExamplesProvider<IReadOnlyList<RoleResponse>>
{
    public IReadOnlyList<RoleResponse> GetExamples() => new List<RoleResponse>
    {
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Administrator"),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Operator")
    };
}
