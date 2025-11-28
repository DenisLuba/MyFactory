namespace MyFactory.MauiClient.Models.Auth;

public record RefreshResponse(
    string AccessToken,
    int ExpiresIn);
