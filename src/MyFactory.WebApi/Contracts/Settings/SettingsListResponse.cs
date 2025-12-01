namespace MyFactory.WebApi.Contracts.Settings;

public record SettingsListResponse(
    string Key,
    string Value,
    string Description
);
