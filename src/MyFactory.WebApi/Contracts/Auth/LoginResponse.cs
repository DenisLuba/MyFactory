namespace MyFactory.WebApi.Contracts.Auth;

public record LoginResponse(
    string AccessToken, 
    string RefreshToken, 
    int ExpiresIn);
