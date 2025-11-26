namespace MyFactory.WebApi.Contracts.Users;

public record UsersCreateResponse(
    Guid Id,
    UserStatus Status
);

public enum UserStatus
{
    Created,
    Updated,
    Deleted
}

