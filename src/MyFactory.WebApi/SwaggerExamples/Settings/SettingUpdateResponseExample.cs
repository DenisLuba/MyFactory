using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Settings;

namespace MyFactory.WebApi.SwaggerExamples.Settings;

public class SettingUpdateResponseExample : IExamplesProvider<SettingUpdateResponse>
{
    public SettingUpdateResponse GetExamples() =>
        new(
            Key: "StandardShiftHours",
            Status: SettingUpdateStatus.Updated
        );
}
