using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Settings;

namespace MyFactory.WebApi.SwaggerExamples.Settings;

public class SettingsUpdateResponseExample : IExamplesProvider<SettingsUpdateResponse>
{
    public SettingsUpdateResponse GetExamples() =>
        new SettingsUpdateResponse(
            Key: "StandardShiftHours",
            Status: SettingsUpdateStatus.Updated
        );
}

