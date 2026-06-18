namespace MyFactory.WebApi.Contracts.Users;

public sealed record UserDetailsResponse(
    Guid Id,
    string Username,
    Guid RoleId,
    string RoleName,
    bool IsActive,
    DateTime CreatedAt);
