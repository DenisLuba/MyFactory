using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Settings;

namespace MyFactory.MauiClient.Services.SettingsServices
{
    public interface ISettingsService
    {
        Task<IReadOnlyList<SettingsListResponse>?> GetAllAsync();
        Task<SettingGetResponse?> GetAsync(string key);
        Task<SettingUpdateResponse?> UpdateAsync(string key, SettingUpdateRequest request);
    }
}