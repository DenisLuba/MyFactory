using MyFactory.WebApi.Contracts.Units;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Units;

public sealed class AddUnitRequestExample : IExamplesProvider<AddUnitRequest>
{
    public AddUnitRequest GetExamples() => new("kg", "Kilograms");
}
