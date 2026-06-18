namespace MyFactory.Application.DTOs.Users;

public sealed record UserListItemDto(
    Guid Id,
    string Username,
    string RoleName,
    bool IsActive,
    DateTime CreatedAt);
