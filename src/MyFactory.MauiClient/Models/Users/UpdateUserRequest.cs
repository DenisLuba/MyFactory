namespace MyFactory.MauiClient.Models.Users;

public sealed record UpdateUserRequest(Guid RoleId, bool IsActive);
