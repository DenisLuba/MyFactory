using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Settings;

namespace MyFactory.MauiClient.Services.SettingsServices
{
    public class SettingsService(HttpClient httpClient) : ISettingsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<SettingsGetResponse>?> GetAllAsync()
            => await _httpClient.GetFromJsonAsync<List<SettingsGetResponse>>("api/settings");

        public async Task<SettingsGetResponse?> GetAsync(string key)
            => await _httpClient.GetFromJsonAsync<SettingsGetResponse>($"api/settings/{key}");

        public async Task<SettingsUpdateResponse?> UpdateAsync(string key, SettingsUpdateRequest request)
            => await _httpClient.PutAsJsonAsync($"api/settings/{key}", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SettingsUpdateResponse>()).Unwrap();
    }
}