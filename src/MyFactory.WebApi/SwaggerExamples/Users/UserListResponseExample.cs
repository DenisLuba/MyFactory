using MyFactory.WebApi.Contracts.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Users;

public sealed class UserListResponseExample : IExamplesProvider<IReadOnlyList<UserListItemResponse>>
{
    public IReadOnlyList<UserListItemResponse> GetExamples() => new List<UserListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            Username: "admin",
            RoleName: "Administrator",
            IsActive: true,
            CreatedAt: new DateTime(2025, 3, 1, 9, 0, 0, DateTimeKind.Utc)),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            Username: "operator",
            RoleName: "Operator",
            IsActive: true,
            CreatedAt: new DateTime(2025, 3, 2, 10, 0, 0, DateTimeKind.Utc))
    };
}
