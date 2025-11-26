namespace MyFactory.WebApi.Contracts.Users;

public record UsersUpdateRequest(
    string UserName,
    string Email,
    string Role,
    bool IsActive
);

