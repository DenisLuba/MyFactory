namespace MyFactory.MauiClient.Models.Auth;

public record RegisterResponse(Guid Id, RegisterStatus Status);

public enum RegisterStatus
{
    Created,
    DuplicateUsername,
    DuplicateEmail,
    WeakPassword
}
