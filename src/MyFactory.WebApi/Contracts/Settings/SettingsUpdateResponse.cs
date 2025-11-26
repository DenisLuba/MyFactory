namespace MyFactory.WebApi.Contracts.Settings;

public record SettingsUpdateResponse(
    string Key,
    SettingsUpdateStatus Status
);

public enum SettingsUpdateStatus
{
    Updated,
    NotFound,
    Failed
}

