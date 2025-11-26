namespace MyFactory.WebApi.Contracts.Users;

public record UsersGetByRoleResponse(
    Guid Id,
    string UserName,
    string Email,
    string Role,
    bool IsActive
);

