namespace MyFactory.MauiClient.Models.Settings;

public record SettingGetResponse(
    string Key,
    string Value,
    string Description
);
