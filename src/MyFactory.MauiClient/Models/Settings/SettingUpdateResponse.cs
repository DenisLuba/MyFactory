namespace MyFactory.MauiClient.Models.Settings;

public record SettingUpdateResponse(
    string Key,
    SettingUpdateStatus Status
);

public enum SettingUpdateStatus
{
    Updated,
    NotFound,
    Failed
}
