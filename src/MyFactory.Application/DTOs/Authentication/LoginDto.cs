namespace MyFactory.Application.DTOs.Authentication;

public record LoginDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);
