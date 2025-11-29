using MyFactory.MauiClient.Models.Settings;

namespace MyFactory.MauiClient.Services.SettingsServices
{
    public interface ISettingsService
    {
        Task<List<SettingsGetResponse>?> GetAllAsync();
        Task<SettingsGetResponse?> GetAsync(string key);
        Task<SettingsUpdateResponse?> UpdateAsync(string key, SettingsUpdateRequest request);
    }
}