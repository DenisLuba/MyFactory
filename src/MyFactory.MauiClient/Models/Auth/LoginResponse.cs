namespace MyFactory.MauiClient.Models.Auth;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn);
