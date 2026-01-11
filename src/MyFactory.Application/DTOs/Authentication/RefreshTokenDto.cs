namespace MyFactory.Application.DTOs.Authentication;

public record RefreshTokenDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);