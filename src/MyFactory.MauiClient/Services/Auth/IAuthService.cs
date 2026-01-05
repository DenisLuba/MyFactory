using MyFactory.MauiClient.Models.Auth;

namespace MyFactory.MauiClient.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<RefreshResponse?> RefreshAsync(RefreshRequest request);
    Task<RegisterResponse?> RegisterAsync(RegisterRequest request);
}
