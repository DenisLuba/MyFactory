using MyFactory.WebApi.Contracts.Units;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Units;

public sealed class AddUnitResponseExample : IExamplesProvider<AddUnitResponse>
{
    public AddUnitResponse GetExamples() => new(Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc3"));
}
