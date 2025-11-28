namespace MyFactory.MauiClient.Models.Auth;

public record RegisterRequest(
    string UserName,
    string Email,
    string Password);
