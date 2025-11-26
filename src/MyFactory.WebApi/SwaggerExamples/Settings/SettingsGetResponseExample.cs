using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Settings;

namespace MyFactory.WebApi.SwaggerExamples.Settings;

public class SettingsGetResponseExample : IExamplesProvider<SettingsGetResponse>
{
    public SettingsGetResponse GetExamples() =>
        new SettingsGetResponse(
            Key: "StandardShiftHours",
            Value: "8",
            Description: "Стандартная длительность смены (часы)"
        );
}

