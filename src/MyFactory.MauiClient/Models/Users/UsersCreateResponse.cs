namespace MyFactory.MauiClient.Models.Users;

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

