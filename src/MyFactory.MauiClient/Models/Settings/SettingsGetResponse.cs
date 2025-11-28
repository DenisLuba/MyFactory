namespace MyFactory.MauiClient.Models.Settings;

public record SettingsGetResponse(
    string Key,
    string Value,
    string Description
);

