namespace MyFactory.WebApi.Contracts.Auth;

public record RegisterResponse(Guid Id, RegisterStatus Status);

public enum RegisterStatus
{
    Created,
    DuplicateUsername,
    DuplicateEmail,
    WeakPassword
}