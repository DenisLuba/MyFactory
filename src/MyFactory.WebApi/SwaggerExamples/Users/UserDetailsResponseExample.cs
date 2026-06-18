using MyFactory.WebApi.Contracts.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public sealed class UserDetailsResponseExample : IExamplesProvider<UserDetailsResponse>
{
    public UserDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Username: "admin",
        RoleId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        RoleName: "Administrator",
        IsActive: true,
        CreatedAt: new DateTime(2025, 3, 1, 9, 0, 0, DateTimeKind.Utc));
}
