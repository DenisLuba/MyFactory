namespace MyFactory.WebApi.Contracts.Users;

public sealed record CreateUserRequest(string Username, string Password, Guid RoleId);
