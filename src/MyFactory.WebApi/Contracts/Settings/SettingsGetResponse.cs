namespace MyFactory.WebApi.Contracts.Settings;

public record SettingsGetResponse(
    string Key,
    string Value,
    string Description
);

