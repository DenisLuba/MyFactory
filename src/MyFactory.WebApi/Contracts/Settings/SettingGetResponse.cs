namespace MyFactory.WebApi.Contracts.Settings;

public record SettingGetResponse(
    string Key,
    string Value,
    string Description
);
