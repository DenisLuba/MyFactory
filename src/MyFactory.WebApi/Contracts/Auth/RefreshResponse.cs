namespace MyFactory.WebApi.Contracts.Auth;

public record RefreshResponse(string AccessToken, int ExpiresIn);

