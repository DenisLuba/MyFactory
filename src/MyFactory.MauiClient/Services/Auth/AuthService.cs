using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Auth;

namespace MyFactory.MauiClient.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public async Task<RefreshResponse?> RefreshAsync(RefreshRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RefreshResponse>();
    }

    public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RegisterResponse>();
    }
}
