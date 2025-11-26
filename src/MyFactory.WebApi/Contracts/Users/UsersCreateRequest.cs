namespace MyFactory.WebApi.Contracts.Users;

public record UsersCreateRequest(
    string UserName,
    string Email,
    string Role,
    string Password
);

