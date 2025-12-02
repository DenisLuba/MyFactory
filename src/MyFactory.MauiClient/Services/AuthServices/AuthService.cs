using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Auth;

namespace MyFactory.MauiClient.Services.AuthServices
{
    public class AuthService(HttpClient httpClient) : IAuthService
    {
        private readonly HttpClient _httpClient = httpClient;

        /*public async Task<LoginResponse?> LoginAsync(LoginRequest request)
            => await _httpClient.PostAsJsonAsync("api/auth/login", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<LoginResponse>()).Unwrap();

        public async Task<RefreshResponse?> RefreshAsync(RefreshRequest request)
            => await _httpClient.PostAsJsonAsync("api/auth/refresh", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<RefreshResponse>()).Unwrap();

        public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
            => await _httpClient.PostAsJsonAsync("api/auth/register", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<RegisterResponse>()).Unwrap();*/
    }
}