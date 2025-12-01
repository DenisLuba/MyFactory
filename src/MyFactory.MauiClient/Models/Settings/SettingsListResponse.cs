namespace MyFactory.MauiClient.Models.Settings;

public record SettingsListResponse(
    string Key,
    string Value,
    string Description
);
