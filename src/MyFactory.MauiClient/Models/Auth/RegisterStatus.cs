namespace MyFactory.MauiClient.Models.Auth;

public enum RegisterStatus
{
    Created,
    DuplicateUsername,
    DuplicateEmail,
    WeakPassword
}
