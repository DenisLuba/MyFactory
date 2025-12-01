using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Settings;

namespace MyFactory.WebApi.SwaggerExamples.Settings;

public class SettingUpdateRequestExample : IExamplesProvider<SettingUpdateRequest>
{
    public SettingUpdateRequest GetExamples() => new(Value: "9");
}
