namespace MyFactory.MauiClient.Models.Users;

public sealed record CreateUserRequest(string Username, string Password, Guid RoleId);
