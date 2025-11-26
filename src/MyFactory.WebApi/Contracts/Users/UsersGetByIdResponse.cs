namespace MyFactory.WebApi.Contracts.Users;

public record UsersGetByIdResponse(
    Guid Id,
    string UserName,
    string Email,
    string Role,
    bool IsActive
);

