using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using MyFactory.MauiClient.Models.Auth;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public Guid? CurrentUserId { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken);

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        await response.EnsureSuccessWithProblemAsync();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result is not null)
        {
            SetSession(result.AccessToken, result.RefreshToken);
        }

        return result;
    }

    public async Task<RefreshResponse?> RefreshAsync(RefreshRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", request);
        await response.EnsureSuccessWithProblemAsync();
        var result = await response.Content.ReadFromJsonAsync<RefreshResponse>();

        if (result is not null)
        {
            SetSession(result.AccessToken, RefreshToken);
        }

        return result;
    }

    public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<RegisterResponse>();
    }

    private void SetSession(string? accessToken, string? refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        CurrentUserId = DecodeUserId(accessToken);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    private static Guid? DecodeUserId(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return null;

        var jwt = handler.ReadJwtToken(token);
        var sub = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub || c.Type == "sub")?.Value;
        return Guid.TryParse(sub, out var guid) ? guid : null;
    }
}
