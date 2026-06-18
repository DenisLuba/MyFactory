namespace MyFactory.MauiClient.Models.Auth;

public record RegisterRequest(
    string UserName,
    Guid RoleId,
    string Password);
