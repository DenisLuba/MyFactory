using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Settings;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Settings;

public class SettingsListResponseExample : IExamplesProvider<IEnumerable<SettingsListResponse>>
{
    public IEnumerable<SettingsListResponse> GetExamples() => new[]
    {
        new SettingsListResponse(
            Key: "StandardShiftHours",
            Value: "8",
            Description: "Стандартная длительность смены (часы)"
        ),
        new SettingsListResponse(
            Key: "Currency",
            Value: "₽",
            Description: "Валюта системы"
        )
    };
}
