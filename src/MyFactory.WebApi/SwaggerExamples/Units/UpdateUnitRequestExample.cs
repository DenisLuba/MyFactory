using MyFactory.WebApi.Contracts.Units;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Units;

public sealed class UpdateUnitRequestExample : IExamplesProvider<UpdateUnitRequest>
{
    public UpdateUnitRequest GetExamples() => new("cm", "Centimeters");
}
