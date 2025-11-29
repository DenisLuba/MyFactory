using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Users;

namespace MyFactory.MauiClient.Services.UsersServices
{
    public class UsersService(HttpClient httpClient) : IUsersService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<UsersGetByRoleResponse>?> GetByRoleAsync(string? role = null)
            => await _httpClient.GetFromJsonAsync<List<UsersGetByRoleResponse>>($"api/users{(role != null ? "?role=" + role : "")}");

        public async Task<UsersGetByIdResponse?> GetByIdAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<UsersGetByIdResponse>($"api/users/{id}");

        public async Task<UsersCreateResponse?> CreateAsync(UsersCreateRequest request)
            => await _httpClient.PostAsJsonAsync("api/users", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<UsersCreateResponse>()).Unwrap();

        public async Task<UsersUpdateResponse?> UpdateAsync(Guid id, UsersUpdateRequest request)
            => await _httpClient.PutAsJsonAsync($"api/users/{id}", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<UsersUpdateResponse>()).Unwrap();

        public async Task<UsersDeleteResponse?> DeleteAsync(Guid id)
            => await _httpClient.DeleteAsync($"api/users/{id}")
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<UsersDeleteResponse>()).Unwrap();
    }
}