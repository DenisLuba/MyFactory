namespace MyFactory.MauiClient.Models.Users;

public sealed record UserListItemResponse(
    Guid Id,
    string Username,
    string RoleName,
    bool IsActive,
    DateTime CreatedAt);
