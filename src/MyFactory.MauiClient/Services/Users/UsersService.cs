using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Users;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Users;

public sealed class UsersService : IUsersService
{
    private readonly HttpClient _httpClient;

    public UsersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<RoleResponse>?> GetRolesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<RoleResponse>>("api/users/roles");
    }

    public async Task<CreateRoleResponse?> CreateRoleAsync(CreateRoleRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/roles", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateRoleResponse>();
    }

    public async Task UpdateRoleAsync(Guid roleId, UpdateRoleRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/users/roles/{roleId}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task RemoveRoleAsync(Guid roleId)
    {
        var response = await _httpClient.DeleteAsync($"api/users/roles/{roleId}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<IReadOnlyList<UserListItemResponse>?> GetUsersAsync(Guid? roleId = null, string? roleName = null)
    {
        var query = new List<string>();
        if (roleId is not null) query.Add($"roleId={roleId}");
        if (!string.IsNullOrWhiteSpace(roleName)) query.Add($"roleName={Uri.EscapeDataString(roleName)}");
        var path = "api/users" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        return await _httpClient.GetFromJsonAsync<List<UserListItemResponse>>(path);
    }

    public async Task<UserDetailsResponse?> GetUserAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<UserDetailsResponse>($"api/users/{id}");
    }

    public async Task<CreateUserResponse?> CreateUserAsync(CreateUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateUserResponse>();
    }

    public async Task UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task DeactivateUserAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/users/{id}/deactivate", null);
        await response.EnsureSuccessWithProblemAsync();
    }
}
