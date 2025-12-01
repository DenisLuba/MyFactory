using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Settings;

namespace MyFactory.WebApi.SwaggerExamples.Settings;

public class SettingGetResponseExample : IExamplesProvider<SettingGetResponse>
{
    public SettingGetResponse GetExamples() =>
        new(
            Key: "StandardShiftHours",
            Value: "8",
            Description: "Стандартная длительность смены (часы)"
        );
}
