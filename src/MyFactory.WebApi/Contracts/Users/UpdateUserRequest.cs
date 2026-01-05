namespace MyFactory.WebApi.Contracts.Users;

public sealed record UpdateUserRequest(Guid RoleId, bool IsActive);
