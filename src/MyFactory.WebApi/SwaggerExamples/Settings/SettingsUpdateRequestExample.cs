using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Settings;

namespace MyFactory.WebApi.SwaggerExamples.Settings;

public class SettingsUpdateRequestExample : IExamplesProvider<SettingsUpdateRequest>
{
    public SettingsUpdateRequest GetExamples() =>
        new SettingsUpdateRequest(Value: "9");
}

