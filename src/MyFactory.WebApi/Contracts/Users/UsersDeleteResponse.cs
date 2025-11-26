namespace MyFactory.WebApi.Contracts.Users;

public record UsersDeleteResponse(
    Guid Id,
    UserStatus Status
);

