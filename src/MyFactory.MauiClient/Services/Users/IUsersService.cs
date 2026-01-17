using MyFactory.MauiClient.Models.Users;

namespace MyFactory.MauiClient.Services.Users;

public interface IUsersService
{
    Task<IReadOnlyList<RoleResponse>?> GetRolesAsync();
    Task<CreateRoleResponse?> CreateRoleAsync(CreateRoleRequest request);
    Task UpdateRoleAsync(Guid roleId, UpdateRoleRequest request);
    Task RemoveRoleAsync(Guid roleId);

    Task<IReadOnlyList<UserListItemResponse>?> GetUsersAsync(Guid? roleId = null, string? roleName = null);
    Task<UserDetailsResponse?> GetUserAsync(Guid id);
    Task<CreateUserResponse?> CreateUserAsync(CreateUserRequest request);
    Task UpdateUserAsync(Guid id, UpdateUserRequest request);
    Task DeactivateUserAsync(Guid id);
}
