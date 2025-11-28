namespace MyFactory.MauiClient.Models.Users;

public record UsersCreateRequest(
    string UserName,
    string Email,
    string Role,
    string Password
);

