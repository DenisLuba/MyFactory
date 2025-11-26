namespace MyFactory.WebApi.Contracts.Users;

public record UsersUpdateResponse(
    Guid Id,
    UserStatus Status
);

