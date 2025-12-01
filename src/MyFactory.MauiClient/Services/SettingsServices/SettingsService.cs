using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Settings;

namespace MyFactory.MauiClient.Services.SettingsServices
{
    public class SettingsService(HttpClient httpClient) : ISettingsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IReadOnlyList<SettingsListResponse>?> GetAllAsync()
            => await _httpClient.GetFromJsonAsync<List<SettingsListResponse>>("api/settings");

        public async Task<SettingGetResponse?> GetAsync(string key)
            => await _httpClient.GetFromJsonAsync<SettingGetResponse>($"api/settings/{key}");

        public async Task<SettingUpdateResponse?> UpdateAsync(string key, SettingUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/settings/{key}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SettingUpdateResponse>();
        }
    }
}